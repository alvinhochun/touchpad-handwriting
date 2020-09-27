The out-of-process touchpad uses a simple asynchronous binary IPC via a named
pipe.


Terminology
---

### Host

The process wanting touchpad events.

### Client

The process which captures hardware touchpad events and passes them to the host
process.


Initialization
---

1. The host process starts the client process with the first argument being
   `run` and the second argument being the name of the named pipe to use. The
   name *must not* include the prefix `\\.\pipe\`.
2. The client process creates a named pipe server with the provided name. The
   named pipe should be created with the parameters `PIPE_ACCESS_DUPLEX`,
   `FILE_FLAG_OVERLAPPED`, `PIPE_TYPE_BYTE` and `PIPE_READMODE_BYTE`.
3. The host process connects to the named pipe server.
4. The client sends a `Protocol Version` message.

The host should attempt to connect to the named pipe for no less than 5 seconds
after starting the client process.

Strictly speaking, the client process that connects to the named pipe is not
required to be the same process that is started by the host. However, it is
recommended that the client process does not terminate as long as the named
pipe is open.


Client Process
---

The client process handles messages received from the host and reports touchpad
events if enabled. Touchpad event messages should be sent to the host
immediately on the events being received by the client process.

### States

The client contains some states that can be controlled by the host, as follows:

#### Enabled state

Controlled by the `Set Enable` command. When the enabled state is toggled, the
client *must* activate or deactivate the touchpad event reporting. Ideally, if
the enabled state is off, then any callbacks or hooks related to event handling
should be disabled or unregistered, but this is not strictly required.

The enabled state should be initially off.

#### Exclusive capture state

Controlled by the `Set Exclusive Capture` command. When this state is on, the
client should try its best to block the touchpad events from reaching the
other applications as mouse events or gestures. Note that this is only best-
effort, so there is no need to go too far to achieve this. For example,
injecting DLLs to all processes is *not* appropriate. Using a low-level mouse
hook is fine, however.

The exclusive capture state should be initially off.

### Command Line Arguments

If the client process is launched without command line arguments, it is free to
provide any functionality.

The client process must support these command line arguments:

#### `run`

If the first argument is `run`, the second argument must be the name of a named
pipe to connect to *without* the prefix `\\.\pipe\`. This starts the touchpad
event reporting process as described in the previous section.

#### `print`

If the first argument is `print`, the second argument should be the name of one
of the available properties. This mode prints the value of a property to
`stdout` and immediately exits. Newlines are optional. If there are multiple
lines in the output, only the first line will be used. The output text should
be encoded in UTF-8.

If an unsupported property name is received, it should print `???` to `stdout`
and exit with an exit code of `2`.

Available properties:

- `supports_v1`: Whether this client supports the protocol version 1. Must be
  one of the following:
    * `0`: Not supported
    * `1`: Supported
- `display_name`: A display name for the touchpad device type that this client
  supports.
- `display_name_***` where `***` is a locale code (^): A localized display
  name as above. If the requested localization is not available, it should fall
  back to English.

(^) locale code: A tag as returned by the [.NET class property
`System.Globalization.CultureInfo.Name`][CultureInfo.Name], which may be in the
format of "*languagecode2*-*country/regioncode2*" or "*languagecode2*".
"*languagecode2*" is a lowercase two-letter code derived from ISO 639-1.
"*country/regioncode2*" is derived from ISO 3166 and usually consists of two
uppercase letters, or a BCP-47 language tag.

[CultureInfo.Name]: https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.name?view=netframework-4.7.2


Messages
---

This section describes the messages for protocol version 1.

Both the host and the client can send messages continuously. There is no
synchronization between the two.

The basic format of a message is as follows:

- byte 0 (len 1) `message_type`: The message type, which is defined by the
  table in the next section.
- byte 1.. (len ??): `message_body`: The message body. The body length is pre-
  defined by the message type.

There is no delimiter between messages.

When an unknown or invalid message is received by either the host or client,
the receiving end must close the named pipe at the nearest opportunity.

### `0x20` Protocol Version

Provides the protocol version. It must be the first message to be sent by a
client and the host must check for it.

- Direction: Client -> Host
- `message_type`: `0x20`, which is the ASCII value for whitespace.
- Message body length: 7 bytes
- Message body content: Must be `[0x54, 0x50, 0x56, 0x30, 0x30, 0x30, 0x31]`,
  which is the ASCII values for the string "TPV0001".

To put it simply, the client literally sends the ASCII string `" TPV0001"` when
connected to the host.

### `0x01` Finger Event

Reports a finger event.

- Direction: Client -> Host
- `message_type`: `0x01`
- Message body length: 6 bytes
- Message body content: (note: byte 0 refers to the first byte of the message
  body)
    - byte 0 (len 1) `state`:
        * `0x00`: Finger released
        * `0x01`: Finger pressed
        * `0x02`: Finger move
    - byte 1 (len 1) `finger_id`: A 8-bit unsigned integer uniquely identifying
      a finger. The id only needs to be unique among the currently active
      fingers, so immediately after a finger is released, its id can be reused
      by another finger. The "primary" finger must have an id of `0`. Note that
      for the purpose of handwriting, only the "primary" finger is used in
      practice, and all the non-"primary" fingers will be ignored.
    - byte 2 and 3 (len 2) `finger_x`: A 16-bit unsigned integer in little
      endian indicating the finger position on the horizontal axis. `0`
      represents the leftmost coordinate while `65535` represents the rightmost
      coordinate and anywhere in between is linearly interpolated. If this
      event is for finger released, this field should contain `0` and must not
      be used by the host.
    - byte 4 and 5 (len 2) `finger_y`: A 16-bit unsigned integer in little
      endian indicating the finger position on the vertical axis. `0`
      represents the topmost coordinate while `65535` represents the bottommost
      coordinate and anywhere in between is linearly interpolated. If this
      event is for finger released, this field should contain `0` and must not
      be used by the host.

### `0xA1` Set Enable

Sets the enable state (see previous sections).

- Direction: Host -> Client
- `message_type`: `0xA1`
- Message body length: 1 byte
- Message body content: Must be one of the following:
    * `0x00`: off
    * `0x01`: on

### `0xA2` Set Exclusive Capture

Sets the exclusive capture state (see previous sections).

- Direction: Host -> Client
- `message_type`: `0xA2`
- Message body length: 1 byte
- Message body content: Must be one of the following:
    * `0x00`: off
    * `0x01`: on


