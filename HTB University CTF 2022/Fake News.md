<!-- @format -->

# Fake News

CTF Event: HTB University CTF
Category: DFIR
Difficulty: Easy
Platform: HackTheBox
Status: Unposted
Tags: wordpress, wordpress-custom-plugin

## Description

> The Magic Informer is our school's newspaper. Our system administrator and teacher, Nick, maintain it. But according to him, his credentials leaked while sharing his screen to present on a course. It is believed that Dark Pointy Hats got access to the Magic Informer, which they use to host their phishing campaign for freshmen people. Given the root folder of the Magic Informer, can you investigate what happened?

## Steps

By downloading the files we are greeted with a WordPress application that was defaced by malicious attackers. looking through the files we were able to identify a custom plugin that was added and encoded with Base64 in `/html/wp-content/plugins/plugin-manager/plugin-manager.php` which when decoded reveals PHP code with part1 of the flag

```php
$part1 = "HTB{C0m3_0n";
```

we then find another obfuscated script block in the file `/html/wp-blogs/2022/11/index.php` which when adding it to an HTML file and opening it in a browser will download an ISO file that contains a ` .exe`` file which we will run the strings command on  `strings ./official_invitation.exe | grep part2`which will reveal the 2nd part of the flag`part2:\_1t_w4s_t00_g00d_t0_b3_tru3}`

## Flag

`HTB{C0m3_0n_1t_w4s_t00_g00d_t0_b3_tru3}`
