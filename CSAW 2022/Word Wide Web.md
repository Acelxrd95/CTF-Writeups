<!-- @format -->

# Word Wide Web

CTF Event: CSAW-2022
Category: Web
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished

## Description

> Isn't the Word Wide Web a fascinating place to be in? Words.. so many words.. all linked... NOTE: The flag doesn't have a wrapper. It needs to be wrapped with curly brackets and please put `CTF` in front of the curly brackets.

### Link `http://web.chal.csaw.io:5010/`

## Steps

when opening the URL you are greeted with a link to another page

![Pasted image 20220909212335.png](Word%20Wide%20Web/Pasted_image_20220909212335.png)

which redirects us to another page full of highlighted words in `<a>` tags but not all of them have an `href` which can be observed through the browser inspector

![Pasted image 20220909212557.png](Word%20Wide%20Web/Pasted_image_20220909212557.png)

The challenge can be solved by clicking on the links till you reach the flag but writing a python script is easier

## Solution

```python
from os import link
import requests
import json
from bs4 import BeautifulSoup

req = requests.Session()
mainlink = "<http://web.chal.csaw.io:5010>"
links = ['/']

def getpage():
    global mainlink, links
    subdir=links.pop()
    links = []
    result = req.get(mainlink+subdir)
    print(f'getting {mainlink+subdir}')
    src = result.content
    soup = BeautifulSoup(src, "lxml")
    a_links = soup.find_all("a")
    links.extend([link.get("href") for link in a_links if link != ""])
    links = [l for l in links if l != None]
    with open("links.json", "w") as jsonfile:
        jsonfile.write(json.dumps(links))
    if links:
        getpage()
    else: print(result.content.decode())

getpage()

```

## Flag

`CTF{w0rdS_4R3_4mAz1nG_r1ght}`
