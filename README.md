TouchPad Handwriting
====================

Handwriting recognition on a notebook computer.


Status
------

This program does not work well with Windows Precision Touchpad support.

This program was originally written at a time when Windows Precision Touchpad
was not really a thing. It works on systems with the Synaptics touchpad driver
installed. While it still works with Windows Precision Touchpad being used, the
touchpad exclusive mode does not block Precision Touchpad events. Also, if the
touchpad is disabled from within Windows Settings, no touchpad events are
provided for the Synaptics API.

It is also worth noting that Synaptics had stopped providing the SDK. While the
drivers still provides the COM interfaces, it is not guaranteed to continue to
be supported.

I do not know how this program can be modified to support Windows Precision
Touchpad since there does not appear to be a simple way to receive raw touchpad
events and capture them exclusively.


License
-------

This source code is released under the "BSD-2-Clause" license. Please refer to
the `LICENSE` file.
