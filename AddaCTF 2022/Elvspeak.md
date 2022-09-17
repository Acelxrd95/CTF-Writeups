# Elvspeak

CTF Event: AddaCTF
Category: Reversing
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished
Tags: hardcoded-key, reversing

## Description

> Can you figure out what the elves have been chatting about lately?
> 

## Steps

when extracting the zip file attached to the challenge we find 728 binary file
trying to disassemble these files with `objdump` doesn't yield any useful functions but a section called Shell code and so we assume that these binaries are all the same function written in assembly

```nasm
./binary0:     file format elf64-x86-64

Disassembly of section .shellcode:

0000000000401000 <.shellcode>:
  401000:	6a 00                	push   $0x0
  401002:	6a 05                	push   $0x5
  401004:	48 89 e7             	mov    %rsp,%rdi
  401007:	48 c7 c0 23 00 00 00 	mov    $0x23,%rax
  40100e:	0f 05                	syscall
  401010:	58                   	pop    %rax
  401011:	58                   	pop    %rax
  401012:	48 8b 44 24 10       	mov    0x10(%rsp),%rax
  401017:	8a 10                	mov    (%rax),%dl
  401019:	80 f2 16             	xor    $0x16,%dl
  40101c:	80 fa 1c             	cmp    $0x1c,%dl
  40101f:	75 10                	jne    0x401031
  401021:	48 c7 c7 00 00 00 00 	mov    $0x0,%rdi
  401028:	48 c7 c0 3c 00 00 00 	mov    $0x3c,%rax
  40102f:	0f 05                	syscall
  401031:	48 c7 c7 01 00 00 00 	mov    $0x1,%rdi
  401038:	48 c7 c0 3c 00 00 00 	mov    $0x3c,%rax
  40103f:	0f 05                	syscall
```

when analyzing the second file we find almost the same assembly code

```nasm
./binary1:     file format elf64-x86-64

Disassembly of section .shellcode:

0000000000401000 <.shellcode>:
  401000:	6a 00                	push   $0x0
  401002:	6a 05                	push   $0x5
  401004:	48 89 e7             	mov    %rsp,%rdi
  401007:	48 c7 c0 23 00 00 00 	mov    $0x23,%rax
  40100e:	0f 05                	syscall 
  401010:	58                   	pop    %rax
  401011:	58                   	pop    %rax
  401012:	48 8b 44 24 10       	mov    0x10(%rsp),%rax
  401017:	8a 10                	mov    (%rax),%dl
  401019:	80 f2 31             	xor    $0x31,%dl
  40101c:	80 fa 7d             	cmp    $0x7d,%dl
  40101f:	75 10                	jne    0x401031
  401021:	48 c7 c7 00 00 00 00 	mov    $0x0,%rdi
  401028:	48 c7 c0 3c 00 00 00 	mov    $0x3c,%rax
  40102f:	0f 05                	syscall 
  401031:	48 c7 c7 01 00 00 00 	mov    $0x1,%rdi
  401038:	48 c7 c0 3c 00 00 00 	mov    $0x3c,%rax
  40103f:	0f 05                	syscall
```

with the exception of the bytes `0x40101b` and `0x40101e` being changed
when analyzing the workflow of the program we can see that it takes an argument which gets XOR'ed by the byte at `0x40101b` them compared to the byte at `0x40101e`
if the value is equal then the program will exit with code 0 and if not equal it will exit with code 1

## Solution

to find out the correct byte needed to get the exit code 0 we need to XOR both bytes at `0x40101b` and `0x40101e` by doing the first 6 binary files manually we can observe that the pattern is the same throughout the rest of them and the resulted string from the first 6 is `\nLorem`
doing the rest manually is doable but tedious, however, we have python for the rescue by using the `pwntools` library we are able to read the bytes at these addresses XOR them together and then append them to the file `flag.txt`

```python
from pwn import *

for i in range(0,729):
    e = ELF(f"./elves/binary{i}")
    n1 = e.read(0x40101b,1)
    n2 = e.read(0x40101e,1)
    print(n1)
    print(n2)
    with open("flag.txt","ab+") as f:
        f.write(xor(n1,n2))

```

which contains the flag we need

```

Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Sit amet dictum sit amet. Purus sit amet volutpat consequat mauris nunc. Convallis posuere morbi leo urna molestie. Congue mauris rhoncus aenean vel elit scelerisque mauris pellentesque. Diam ut venenatis tellus in metus vulputate. Sed odio morbi quis commodo. Pulvinar neque laoreet suspendisse interdum. Aenean sed adipiscing diam donec adipiscing tristique
Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Sit amet dictum sit amet. Purus sit amet volutpat consequat mauris nunc. Convallis posuere morbi leo urna molestie. Congue mauris rhoncus aenean vel elit scelerisque mauris pellentesque. Diam ut venenatis tellus in metus vulputate. Sed odio morbi quis commodo. Pulvinar neque laoreet suspendisse interdum. Aenean sed adipiscing diam donec adipiscing tristique risus nec feugiat. The flag is ctf{L0r3m_1p5um_d010r_s1t_4m3t}. Tempus iaculis urna id volutpat lacus. Libero nunc consequat interdum varius. Nibh nisl condimentum id venenatis a condimentum vitae. Porta non pulvinar neque laoreet suspendisse.
```

## Flag

`ctf{L0r3m_1p5um_d010r_s1t_4m3t}`