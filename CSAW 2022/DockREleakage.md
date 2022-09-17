# DockREleakage

CTF Event: CSAW-2022
Category: Reversing
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished
Tags: credentials-in-history

## Description

> A breach occurred, and some files have been leaked. One of the leaked files named `dockREleakage.tar.gz` contains an image of one of the company’s components. An anonymous hacker has reached out to me and beware me that there is some serious mistake in my build image process. The hacker implies that sensitive information should be handled carefully. However, I couldn’t find the mistake by myself. Please help me!
> 

## Steps

by extracting the `dockREleakage.tar.gz` inside it we find that it contains docker folders and 2 JSON files by inspecting both files the file `acbb216b17482071caca135101282177f6ffed7b8ee0bfc5323aae103c216d74.json` contained the Docker history from which we can see an entry that looked like Base64

```json
{
	"created": "2022-09-03T07:46:12.680399343Z",

	"created_by": "/bin/sh -c echo \"ZmxhZ3tuM3Yzcl9sMzR2M181M241MTcxdjNfMW5mMHJtNDcxMG5fdW5wcjA=\" \u003e /dev/null",
	
	"empty_layer": true
},
```

when decoding with Base64 we get the first half of the flag `flag{n3v3r_l34v3_53n5171v3_1nf0rm4710n_unpr0` and by extracting the zips found in the layer folders we can find the 2nd part of the flag in a .txt flag file `73c73d_w17h1n_7h3_d0ck3rf1l3}` when added together give us the flag

## Flag

 `flag{n3v3r_l34v3_53n5171v3_1nf0rm4710n_unpr073c73d_w17h1n_7h3_d0ck3rf1l3}`