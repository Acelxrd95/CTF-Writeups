<!-- @format -->

# Anya Gacha

CTF Event: CSAW-2022
Category: Reversing
Difficulty: Easy
Platform: CTFtime
Status: Rooted/Finished
Tags: Unity

## Description

> It is a Gacha game where the player spend coins to do lucky draw. If they can get the rare character Anya, she will tell the player our flag!

## Steps

### Analysis

After extracting the downloaded zip file we can observe that we have a Unity game binary and DLLs when we launch the game there is only one button to spend 10 of the game’s currency to make a wish hoping to get Anya, but this won’t happen because we only have 100 currency and Anya is guaranteed with 1000 wishes

![Pasted image 20220910045217.png](Anya%20Gacha/Pasted_image_20220910045217.png)

We opened the game’s DLL `extractedfolder\AnyaGacha_Data\Managed\Assembly-CSharp.dll` in dnSpy in order to mod the file

![Pasted image 20220910045500.png](Anya%20Gacha/Pasted_image_20220910045500.png)

we noticed the functions `Start`, `wish` and `Upload` to be of interest after observing each function we were able to deduce that the program initializes itself by calling the `Start` function which sets the currency value to 100 and encodes the string `wakuwaku` to ASCII bytes

```csharp
private void Start() {        
	Debug.Log("Main Logic Starts");
	this.counter = Encoding.ASCII.GetBytes("wakuwaku");
	this.value_masker = Random.Range(1, 999);
	this.value = this.mask_value(100);
	Debug.Log(Convert.ToBase64String(this.mySHA256.ComputeHash(this.counter)));
}
```

when the wish button is clicked the `wish` function is called which deducts 10 currency and sets the counter to the hash of the previous counter then calls the `Upload` function

```csharp
public void wish(){    
	int num = this.unmask_value(this.value);
	if (num < 10)    
	{        
		this.insufficient_value();
		return;    
	}    
	num -= 10;
	this.value = this.mask_value(num);
	this.counter = this.mySHA256.ComputeHash(this.counter);
	this.loading.SetActive(true); 
	base.StartCoroutine(this.Upload());
}
```

From the name of the function, we can assume that some data is sent to a server that will return the flag when the data is correct

```csharp
private IEnumerator Upload() {   
	WWWForm wwwform = new WWWForm();
	string str = Convert.ToBase64String(this.counter);
	wwwform.AddField("data", str);
	UnityWebRequest www = UnityWebRequest.Post(this.server, wwwform);
	Debug.Log("Posted: " + str);
	yield return www.SendWebRequest();
	if (www.result != UnityWebRequest.Result.Success){        
		Debug.Log(www.error);    
	} else {       
		this.loading.SetActive(false); 
		string text = www.downloadHandler.text;
		if (text == "") {            
			this.fail();
		} else {
			this.succeed(text); 
		}
	}    
	yield break;
}
```

the data uploaded is the counter variable and if no flag is received it will call the `fail` function else it would call the `succeed` function

### Solution

Now that we have analyzed the program we have 2 main objectives

1. Prevent the program from running out of currency
2. Get the correct counter

To achieve the first objective we can edit the `Start` function to either have 10000 currency

```csharp
private void Start() {        
	Debug.Log("Main Logic Starts");
	this.counter = Encoding.ASCII.GetBytes("wakuwaku");
	this.value_masker = Random.Range(1, 999);
	this.value = this.mask_value(10000);
	Debug.Log(Convert.ToBase64String(this.mySHA256.ComputeHash(this.counter)));
}
```

or edit the `wish` function to increment the currency counter or remove the decrement completely

```csharp
public void wish(){    
	int num = this.unmask_value(this.value);
	if (num < 10)    
	{        
		this.insufficient_value();
		return;    
	}    
	num += 10;
	this.value = this.mask_value(num);
	this.counter = this.mySHA256.ComputeHash(this.counter);
	this.loading.SetActive(true); 
	base.StartCoroutine(this.Upload());
}
```

We can then keep clicking the button till we get Anya however this is a tedious boring process, so the easier way would be to edit the `Upload` function to call the `wish` function on failure instead of the `fail` function

```csharp
private IEnumerator Upload() {   
	WWWForm wwwform = new WWWForm();
	string str = Convert.ToBase64String(this.counter);
	wwwform.AddField("data", str);
	UnityWebRequest www = UnityWebRequest.Post(this.server, wwwform);
	Debug.Log("Posted: " + str);
	yield return www.SendWebRequest();
	if (www.result != UnityWebRequest.Result.Success){        
		Debug.Log(www.error);    
	} else {       
		this.loading.SetActive(false); 
		string text = www.downloadHandler.text;
		if (text == "") {            
			this.wish();
		} else {
			this.succeed(text); 
		}
	}    
	yield break;
}
```

we then need to save the module in order to update the DLL file, and we are ready to launch the game and click the Wish button the game will then keep loading

![Pasted image 20220910051716.png](Anya%20Gacha/Pasted_image_20220910051716.png)

until we successfully get the flag

![Pasted image 20220910051845.png](Anya%20Gacha/Pasted_image_20220910051845.png)

## Flag

`flag{@nya_haha_1nakute_5amishii}`
