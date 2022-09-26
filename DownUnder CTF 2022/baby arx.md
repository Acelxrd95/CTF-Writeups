# baby arx

CTF Event: DownUnderCTF
Category: Crypto
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished
Tags: brute-force

## Description

> I heard that add-rotate-xor are good operations for a cipher so I tried to make my own…
> 

## Steps

The challenge is a python script that reads the flag as bytes then initializes the class baby_arx with the flag as the key and then runs it through the cipher by calling the method b and then prints the flag which is commented out at the bottom of the file

```python
class baby_arx():
    def __init__(self, key):
        assert len(key) == 64
        self.state = list(key)

    def b(self):
        b1 = self.state[0]
        b2 = self.state[1]
        b1 = (b1 ^ ((b1 << 1) | (b1 & 1))) & 0xff
        b2 = (b2 ^ ((b2 >> 5) | (b2 << 3))) & 0xff
        b = (b1 + b2) % 256
        self.state = self.state[1:] + [b]
        return b

    def stream(self, n):
        return bytes([self.b() for _ in range(n)])

FLAG = open('./flag.txt', 'rb').read().strip()
cipher = baby_arx(FLAG)
out = cipher.stream(64).hex()
print(out)

# cb57ba706aae5f275d6d8941b7c7706fe261b7c74d3384390b691c3d982941ac4931c6a4394a1a7b7a336bc3662fd0edab3ff8b31b96d112a026f93fff07e61b
```

by analyzing the ciphered flag and creating a dummy test flag starting with the normal DUCTF{ we get the first 12 characters from the cipher correct meaning that we can brute force the flag by writing a loop that compares each character individually.

## Solution

```python
from string import printable

class baby_arx:
    def __init__(self, key):
        assert len(key) == 64
        self.state = list(key)

    def b(self):
        b1 = self.state[0]
        b2 = self.state[1]
        b1 = (b1 ^ ((b1 << 1) | (b1 & 1))) & 0xFF
        b2 = (b2 ^ ((b2 >> 5) | (b2 << 3))) & 0xFF
        b = (b1 + b2) % 256
        self.state = self.state[1:] + [b]
        return b

    def stream(self, n):
        return bytes([self.b() for _ in range(n)])

def split_out(out):
    return [out[i : i + 2] for i in range(0, len(out), 2)]

FLAG = "DUCTF{" + "?" * 57 + "}"
FLAGLIST = [c for c in FLAG]
FLAG = FLAG.encode()
expected = split_out(
    "cb57ba706aae5f275d6d8941b7c7706fe261b7c74d3384390b691c3d982941ac4931c6a4394a1a7b7a336bc3662fd0edab3ff8b31b96d112a026f93fff07e61b"
)
out = ""

while out != expected:
    # attempt to bruteforce
    found = True
    i = 0
    location = FLAG.find(b"?")
    while found and i < len(printable):
        FLAGLIST[location] = printable[i]
        FLAG = "".join(FLAGLIST).encode()
        # encrypt the flag
        guess = ""
        cipher = baby_arx(FLAG)
        out = cipher.stream(64).hex()
        out = split_out(out)

        # print the progress
        print(
            f'{"".join(out[location])} {"".join(expected[location])} {"".join(FLAGLIST)}',
        )
        if out[location - 1] == expected[location - 1]:
            found = False
            FLAG = "".join(FLAGLIST).encode()
        i += 1
```

## Flag

`DUCTF{i_d0nt_th1nk_th4ts_h0w_1t_w0rks_actu4lly_92f45fb961ecf420}`