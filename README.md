[![No Maintenance Intended](https://img.shields.io/badge/No%20Maintenance%20Intended-x-red.svg?style=flat-square&longCache=true)](http://unmaintained.tech/)


# Combinify

Combinify is a simple C# (.Net4) app that monitors a list of css files and then combines and minifies them when their LastWriteTime attribute changes. 
Setup file can be found [here](https://github.com/belsrc/Combinify/downloads "Setup file download")

### Very basic feature set

* Minify a single file
* Watches a list of files
  * Add directory to watch list
  * Add single file to watch list
  * Move file up or down watch list (Files are processed in order)
  * Remove file from watch list
* Combine all watched files on css file save
* Combine and minify all watched files on css file save
* Save all listed css files as a project
* Load project to populate watch list

### License
Combinify is released under a BSD 3-Clause License

Copyright &copy; 2012, Bryan Kizer
All rights reserved. 

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions are 
met: 

* Redistributions of source code must retain the above copyright notice, 
  this list of conditions and the following disclaimer.
* Redistributions in binary form must reproduce the above copyright notice,
  this list of conditions and the following disclaimer in the documentation
  and/or other materials provided with the distribution.
* Neither the name of the Organization nor the names of its contributors 
  may be used to endorse or promote products derived from this software 
  without specific prior written permission. 
  
THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS 
IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED 
TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT 
HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING 
NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
