# PlayCountFramework
A framework to catch some video of series's play count data on some video platform like Iqiyi(iqiyi.com), Tencent(v.qq.com) and etc.

You can use another repo ADM to help you managing the dependences of the framework.

The core is project Framework
It provide default Contract, CountHelper, NameSwitcher and PresentHelper.
You can inhert them or develop your own.

Seek project contains component with which, you can free search any video series on those video platform. Currently, those CountHelper inhert from CountHelper

Present Project is the component to help you present result in your program.

DefaultSeek include a default config of what data we currently need. Depends on Seek and Present.

TextRecorder is a sample that show result in text files.