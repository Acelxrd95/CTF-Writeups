<!-- @format -->

# Our Spy In New Terrain (OSINT)

CTF Event: CSAW-2022
Category: OSINT
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished
Tags: OSINT

## Description

> It is up to you to determine who The Enemy is going to attack next and when they plan to strike! Their newest recruit is on Twitter as DarkRoom8109. Good luck.

### Link `nc misc.chal.csaw.io 5005`

## Steps

By going on Twitter to the user `https://twitter.com/darkroom8109` we get the first answer for the question asked on the server about the Twitter creation date: `08/2022`

![Pasted image 20220909191436.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220909191436.png>)

we are then asked for his GitHub account and we get a clue by surveying his tweets that he deleted a tweet at around August 25th

![Pasted image 20220909191546.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220909191546.png>)

From here we can try to go on Wayback Machine to check for the deleted tweet that was mentioned
`https://web.archive.org/web/20220820145418/<https://twitter.com/darkroom8109`>
where we are able to locate a tweet on the 17th of August mentioning his github username `spyduhman`

![Pasted image 20220909191834.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220909191834.png>)

where we can find one repository called Chat-App `https://github.com/spyduhman/Chat-App`
and if we look inside the server file we can find that the chat is logged in `log.txt` which can be accessed in previous commits containing the following data

```
Server: bit.ly/evilevilinfo
Client: Roger.
```

when the link is opened we get redirected to a WAV file on mega containing Morse code that says the following:

```
HELLO EVIL AGENT YOUR NEXT TARGET IS A BANK THE BANK'S BIN NUMBER IS 452234 THE TARGETS SWIFT CODE IS YOUR PASSWORD FOR MORE INSTRUCTIONS VISIT BIT.LY SLASH OSINTSEC GOOD LUCK
```

from which we learn 2 things the bank's bin `452234` when analyzed gives us that the bank is based in `Canada` through `https://quickbinlookup.com/?binquery=452234`

![Pasted image 20220909192912.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220909192912.png>)

and the [bit.ly](http://bit.ly/) link `bit.ly/osintsec` which redirects us to a mega PDF file `YoureSoClose.pdf`
which requires the Swift Code in order to open which can be found using the bin
`https://wise.com/us/swift-codes/?country=canada&bank=TORONTO-DOMINION+BANK%2C+THE&city=Toronto#results`
we choose the second option and get the swift code `TDOMCATTTOR`

![Pasted image 20220909193954.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220909193954.png>)

Which unlocks the PDF for us containing the following picture

![Pasted image 20220909194939.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220909194939.png>)

When we open the PDF and check the file properties we can see that the title says:

![Pasted image 20220909234358.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220909234358.png>)

Which is the last answer to the questions

## Solution

![Pasted image 20220910002619.png](<Our%20Spy%20In%20New%20Terrain%20(OSINT)/Pasted_image_20220910002619.png>)

```
08/2022
spyduhman
log.txt
Canada
TDOMCATTTOR
Copyright Infringement

```

## Flag

`flag{C0N6r475463N7600DW0rKN3X771M3N0PU811C53rV3r}`
