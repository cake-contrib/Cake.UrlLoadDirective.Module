# Cake.UrlLoadDirective.Module
A Cake module for loading cake files from arbitrary URL's via the `#load` directive

By default, the `#load` preprocessor directive only supports local file system paths.

This module will allow cake script files from any url to be loaded, for example:

```
#load url:https://someurl.com/somefile.cake
```

Notice the `url:` in front of the actual url.  This is required to indicate to the preprocessor directive that you want to load a url.

# Installation

Installing a Cake Module is pretty simple.  Just add a file like this:
`./tools/Modules/packages.config`

With the content:
```xml
<?xml version="1.0" encoding="utf-8"?>
<packages>
	<package id="Cake.UrlLoadDirective.Module" version="1.0.2" />
</packages>
```

You should now be all set and able to use it in your scripts!

# License

MIT License

Copyright (c) 2017 Jonathan Dick

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
