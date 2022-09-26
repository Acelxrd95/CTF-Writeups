# Source Provided

CTF Event: DownUnderCTF
Category: Reversing
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished
Tags: hardcoded-key, reversing

## Description

> In this reversing challenge, a source is provided.
> 

## Steps

In this reversing challenge, we get provided a binary and its source code in assembly

```nasm
SECTION .data
c db 0xc4, 0xda, 0xc5, 0xdb, 0xce, 0x80, 0xf8, 0x3e, 0x82, 0xe8, 0xf7, 0x82, 0xef, 0xc0, 0xf3, 0x86, 0x89, 0xf0, 0xc7, 0xf9, 0xf7, 0x92, 0xca, 0x8c, 0xfb, 0xfc, 0xff, 0x89, 0xff, 0x93, 0xd1, 0xd7, 0x84, 0x80, 0x87, 0x9a, 0x9b, 0xd8, 0x97, 0x89, 0x94, 0xa6, 0x89, 0x9d, 0xdd, 0x94, 0x9a, 0xa7, 0xf3, 0xb2

SECTION .text

global main

main:
    xor rax, rax  ; rax = 0
    xor rdi, rdi ; rdi = 0
    mov rdx, 0x32 ; rdx = 0x32
    sub rsp, 0x32 ; rsp = rsp - 0x32
    mov rsp, rsi ; rsi = rsp
    syscall ; sys_read(0, rsi, rdx)

    mov r10, 0 ; r10 = 0
l:
    movzx r11, byte [rsp + r10] ; r11 = rsp[r10]
    movzx r12, byte [c + r10] ; r12 = c[r10]
    add r11, r10 ; r11 = r11 + r10
    add r11, 0x42 ; r11 = r11 + 0x42
    xor r11, 0x42 ; r11 = r11 ^ 0x42
    and r11, 0xff ; r11 = r11 & 0xff
    cmp r11, r12 ; r11 == r12
    jne b ; if r11 != r12, jump to b

    add r10, 1 ; r10 = r10 + 1
    cmp r10, 0x32 ; r10 == 0x32
    jne l ; if r10 != 0x32, jump to l

    mov rax, 0x3c ; rax = 0x3c
    mov rdi, 0 ; rdi = 0
    syscall ; exit(0)

b:
    mov rax, 0x3c ; rax = 0x3c
    mov rdi, 1 ; rdi = 1
    syscall ; exit(1)
```

by analyzing the assembly code we can see that the user takes input from the user and stores it in `r11` then 

`add the value of r10 -> add 0x42 -> xor 0x42 -> and 0xff` then compares the user input to the values in the data variable `c`

## Solution

there are 2 different ways to get the flag by either reversing the operation or brute forcing each character and comparing it against the values stored in the memory

```python
codes = [0xC4,0xDA,0xC5,0xDB,0xCE,0x80,0xF8,0x3E,0x82,0xE8,0xF7,0x82,0xEF,0xC0,0xF3,0x86,0x89,0xF0,0xC7,0xF9,0xF7,0x92,0xCA,0x8C,0xFB,0xFC,0xFF,0x89,0xFF,0x93,0xD1,0xD7,0x84,0x80,0x87,0x9A,0x9B,0xD8,0x97,0x89,0x94,0xA6,0x89,0x9D,0xDD,0x94,0x9A,0xA7,0xF3,0xB2,
]

flag = ""

# SOLUTION 1 by reversing the operation
#####################################
for i in range(len(codes)):
    codes[i] = codes[i] & 0xFF
    codes[i] = codes[i] ^ 0x42
    codes[i] = codes[i] - 0x42
    codes[i] = codes[i] - i
    flag += chr(codes[i])
#####################################

# SOLUTION 2 by bruteforcing the original chars
#####################################
# for i in range(len(codes)):
#     code = codes[i]
#     for j in range(0x100):
#         ans = j
#         ans += i
#         ans += 0x42
#         ans ^= 0x42
#         ans = ans & 0xFF
#         if ans == code:
#             flag += chr(j)
#             break
#####################################
print(flag)
```

## Flag

`DUCTF{r3v_is_3asy_1f_y0u_can_r34d_ass3mbly_r1ght?}`