MineML
======

A multithread CPU based bitcoin miner written in F#. 
At the moment it's a slow implementation, but the class structure offers the possibility to implement different type of MinerThread using different processing methods (opencl, cuda, or sha256 dedicated hardware).


Requirements
============
  * Mono 3.0 (.net api 4.0)


TODO
====
  * Statistics
  * GUI interface
  * OpenCL thread implementation
