<!-- @format -->

# One of us

CTF Event: HTB University CTF
Category: DFIR
Difficulty: Medium
Platform: HackTheBox
Status: Unposted
Tags: Office Document, Visual Basic Script

## Description

> Dark Pointy Hats are causing trouble again. This time, they have targeted Invisible Shields and the protectors of the forbidden spells. They developed a specific spyware that aims to get access to the forbidden spells server. We managed to retrieve a sample of the spyware and suspicious mail that seems to be produced by the spyware. Can you analyze the provided files and find out what happened?

## Steps

for this challenge we were provided a text file that contained what looks like an encrypted email and `.docm` we started by running oletools’ `olevba` on the `docm` file using the arguments `olevba --deobf .\invisible_shields.docm` to check for VBA scripts and dump them which the command succeeded in doing and output an obfuscated VBA script to the terminal

[original.vb](One%20of%20us/original.vb)

trying to de-obfuscate the file was a real challenge that took countless hours from our team but the key to de-obfuscate the file was to understand the most repeated function `nkalPYSrDkoirG`

<aside>
💡 the de-obfuscation methodology is essentially reverse engineering and a lot of guessing what stuff does. it takes time, skill, practice and sometimes teamwork to get it right

</aside>
To understand the function `nkalPYSrDkoirG` we needed to understand and rename other variables and names such as `ovLKcDvvuvaxVc` which was key to revealing the functionality of most of the script

The result of the function `nkalPYSrDkoirG` was

```visual-basic
' while deobfuscating the code we tried to keep it as close to python syntax as possible to allow as to understand it quicker
Private Function nkalPYSrDkoirG (in_array As Variant, offset As Integer)
    Dim final_string As String
    Dim byte_list() As Byte
    byte_list = base64_decode(ActiveDocument.Variables("gtrxGyKtbDzUEDng"))
    final_string = ""
    For i = 0 To length(in_array)
        final_string = final_string + Chr(byte_list(i + offset) ^ in_array(i))
    Next
    nkalPYSrDkoirG= final_string
End Function
```

which would get variable `gtrxGyKtbDzUEDng` from the document and base64 decode it then uses it as a key for an XOR operation.
To get the variable `gtrxGyKtbDzUEDng` we unzipped the document using the command `unzip ./invisible_shields.docm` and we were able to uncover the variable using `grep -R gtrxGyKtbDzUEDng ./invisible_shields` which had the value

```
eNS7GlezU9snp3ciGjUJ9HD0eo5arrhaNii/Jgh7Rq38gvvpitv8AHreIuCHDbXhLd1BlLceamykizs8G02DzoP5bZm0PWZkL80S8MfgzZKkTAWqU3oSdton381J023oFIgmK5mEI4c+F85DAOx+mOkrnEbqMaOzJ4EQ4lSM2LfCgqS7AXQDbwipi5KrDBRkfKO8Me3+6MQ5g/XK6b6e2W5HvaCGoWDe6P2crp90G3GTh0kAemmwX1OOhX1IaAeKe8GbBiyp++2WTalzSf1vCviI5a+jcyRw26L8DP6i4urW+YP902QZa43DZ6A+d8Zh438OogAeuuBaNXUgPEgPQpQaca+NDHco7sYPzmI4Fb1XJU9SS1xGw1gU06x8vZ2w6u8oqnQN/xxTvGjxXUV+X9fnxUGQsg64B85ekF+DPeJD/92LHqrK2wVSVYgHGqvwKY/Yshfu9t2fl74o7KDTFATUJa1AHmy9zsNuZPvvwbwG9iD1cHFJLnLemhWN+6vMoQiO/xUIYMWKGQk2D8+RiSvhlptUw2195E3e7K40WnXNLSyAMvW+ngfplr9T23xyapsNo8gz/MOdw0KWMB868kW9kAGQ5IXWPHGaE7H8hWB3t+1K5H861yr7u5BgZIUby3VU0gKV8EH2c0Gl7rCa6sFbiTtCXmV3r1A+Fm3vBMCG19X2YPN62VpHhRMobsfSEl5TezlLWYVA/HNP6G5VX8+sxdTdQOyk84SGtm8I5Ss6kL4bs/+zw/VdcaXr8IZSa5rsmSgRC4+mLHhPSBTZODowjHPJOZK++rnkqLWQTzIRiiRZZVXeSoVEIGSla44WBR7x2xJABJrRzCxKUg+ryslthKXjteBuF9JZZovMADo9uRVgtu7XYVahg9ujIR310KWMMKlr+rzsLAvvlMLPHGVrG8LDoHrbURxqjPlU3a5OppL//jZIRKGTHO353w8HNR/ly3P3Nw==
```

we then wrote a python script to help us quickly de-obfuscate the contents and usages of the function `nkalPYSrDkoirG` to uncover the strings in the file

```python
import base64

def nkalPYSrDkoirG(in_array, offset):
    final_string = ""
    byte_list = base64.b64decode(
        "eNS7GlezU9snp3ciGjUJ9HD0eo5arrhaNii/Jgh7Rq38gvvpitv8AHreIuCHDbXhLd1BlLceamykizs8G02DzoP5bZm0PWZkL80S8MfgzZKkTAWqU3oSdton381J023oFIgmK5mEI4c+F85DAOx+mOkrnEbqMaOzJ4EQ4lSM2LfCgqS7AXQDbwipi5KrDBRkfKO8Me3+6MQ5g/XK6b6e2W5HvaCGoWDe6P2crp90G3GTh0kAemmwX1OOhX1IaAeKe8GbBiyp++2WTalzSf1vCviI5a+jcyRw26L8DP6i4urW+YP902QZa43DZ6A+d8Zh438OogAeuuBaNXUgPEgPQpQaca+NDHco7sYPzmI4Fb1XJU9SS1xGw1gU06x8vZ2w6u8oqnQN/xxTvGjxXUV+X9fnxUGQsg64B85ekF+DPeJD/92LHqrK2wVSVYgHGqvwKY/Yshfu9t2fl74o7KDTFATUJa1AHmy9zsNuZPvvwbwG9iD1cHFJLnLemhWN+6vMoQiO/xUIYMWKGQk2D8+RiSvhlptUw2195E3e7K40WnXNLSyAMvW+ngfplr9T23xyapsNo8gz/MOdw0KWMB868kW9kAGQ5IXWPHGaE7H8hWB3t+1K5H861yr7u5BgZIUby3VU0gKV8EH2c0Gl7rCa6sFbiTtCXmV3r1A+Fm3vBMCG19X2YPN62VpHhRMobsfSEl5TezlLWYVA/HNP6G5VX8+sxdTdQOyk84SGtm8I5Ss6kL4bs/+zw/VdcaXr8IZSa5rsmSgRC4+mLHhPSBTZODowjHPJOZK++rnkqLWQTzIRiiRZZVXeSoVEIGSla44WBR7x2xJABJrRzCxKUg+ryslthKXjteBuF9JZZovMADo9uRVgtu7XYVahg9ujIR310KWMMKlr+rzsLAvvlMLPHGVrG8LDoHrbURxqjPlU3a5OppL//jZIRKGTHO353w8HNR/ly3P3Nw=="
    )
    for i in range(len(in_array)):
        final_string = final_string + chr(byte_list[i + offset] ^ in_array[i])
    return final_string

def garbage_to_string(string):

    print(eval(string.replace("Array", "").replace("&", "+")))

if __name__ == "__main__":
    garbage_to_string(
        "nkalPYSrDkoirG(Array((27 ^ 96), (69 ^ 194), 173, (126 + 125), 121, (59 ^ 163), (34 ^ 135), ((35 ^ 6) + 94), 51, ((12 ^ 0) + (87 ^ 9)), (17 + (2 ^ 4)), 231, (20 + (21 ^ 8)), ((0 ^ 3) + 110), 196, (169 + 19), 30, 231, (66 + (7 ^ 106)), (210 ^ 51), (97 + 17), (142 + (5 ^ 26)), (9 + (98 ^ 167)), 191, 220, (31 ^ 205), ((42 ^ 91) + 120), (94 + 0), 135), (51 ^ 371)) & nkalPYSrDkoirG(Array((37 ^ 176), ((6 ^ 47) + 190), 36), ((161 ^ 123) + (40 ^ 171)))"
    )
```

the final result of de-obfuscating the file

[deobfuscated.vb](One%20of%20us/deobfuscated.vb)

reveals to us that the function `ydcyecOpBU` encrypts a message using `Rijndael256`

```visual-basic
Function ydcyecOpBU(arg0)
    Dim encrypted_text As Byte
    Dim IV  As String
    IV = generate_random_string(32)
    Dim utf8_enc, igFwXpLeArakc, zrjycZwtKFJvAu, IVdDfkxSWPOTi As Object
    Dim kSNpGaWBeFRbpl() As Byte
    Set rijndael_man = CreateObject("System.Security.Cryptography.RijndaelManaged")
    Set utf8_enc = CreateObject("System.Text.UTF8Encoding")
    rijndael_man.KeySize = 256
    rijndael_man.QrbstMncnU = 256
    rijndael_man.mode = 1 'CBC
    rijndael_man.Key = CreateObject("System.Text.UTF8Encoding").GetBytes_4("8xppg2oX68Bo6koL7hwSeC8bCEWvk540")
    rijndael_man.IV = CreateObject("System.Text.UTF8Encoding").GetBytes_4(IV)
    encoded_arg0 = CreateObject("System.Text.UTF8Encoding").GetBytes_4(arg0)
    encrypted_text = rijndael_man.CreateEncryptor().TransformFinalBlock((encoded_arg0), 0, length(encoded_arg0))
    ydcyecOpBU= IV & "|" & base64_encode(encrypted_text)
End Function
```

which revealed to us that the email contents consisted of the Initialization Vector and the base64 encoded secret message separated by a `|` and the key was revealed to us in the function `ydcyecOpBU`

```python
Key = "8xppg2oX68Bo6koL7hwSeC8bCEWvk540"
IV = "*twGsy*#p7XY8CT4N3RpGq5xDzL7EMHW"
CipherText = "F02fGjYTWhdk3JYn2nntOcU56fnU0YD4prneoaPxbsNIcMgcwsFFGWifg7tNNkohHj9nZRTWJDg/BcnUpTuKynaTtMg9fnOnhjYmg++Q6pklR9Zt0s2vzVu2FMJxO+xBaQrONSPvPg5sd2qRtAkrCa4ikKuKwg38QA7v+wseZRrx37P2sIiellwVcWFMRQCZtlE6bdN14JKmXn+GeXFIP51KHOCR3qd34NgzcGuLySbH9ZGzldLZWagnIcAFKTP9"
```

we then translated the key, IV and Cipher Text to Hex to decrypt the text

```python
Key = 0x3878707067326f583638426f366b6f4c3768775365433862434557766b353430
IV = 0x2a74774773792a2370375859384354344e33527047713578447a4c37454d4857
CipherText = 0x174d9f1a36135a1764dc9627da79ed39c539e9f9d4d180f8a6b9dea1a3f16ec34870c81cc2c14519689f83bb4d364a211e3f676514d624383f05c9d4a53b8aca7693b4c83d7e73a786362683ef90ea992547d66dd2cdafcd5bb614c2713bec41690ace3523ef3e0e6c776a91b4092b09ae2290ab8ac20dfc400eeffb0b1e651af1dfb3f6b0889e965c1571614c450099b6513a6dd375e092a65e7f867971483f9d4a1ce091dea777e0d833706b8bc926c7f591b395d2d959a82721c0052933fd
```

all that was left was to enter the data into an online Rijndael decryption tool at: [http://rijndael.online-domain-tools.com/](http://rijndael.online-domain-tools.com/) which revealed to us the plaintext

```
Dear Austin,

I created an account for you in the forbidden spells server as you wished.

Your credentials are:

username: paustin
password: HTB{th3s3_sp3lls_4r3_t00_d4ng3r0us}

Sincerely,
P
```

## Flag

`HTB{th3s3_sp3lls_4r3_t00_d4ng3r0us}`
