# RTM Client C# SDK

[TOC]

## Depends

* [msgpack-csharp](https://github.com/highras/msgpack-csharp)

* [fpnn-sdk-csharp](https://github.com/highras/fpnn-sdk-csharp/tree/master)

### Compatibility Version:

C# .Net Standard 2.0

## Usage

### Using package

	using com.fpnn.rtm;

### Init

#### FPNN SDK (Unity is REQUIRED, other is optional)

	using com.fpnn;
	ClientEngine.Init();
	ClientEngine.Init(Config config);

#### RTM SDK (Unity is REQUIRED, other is optional)

	using com.fpnn.rtm;
	RTMControlCenter.Init();
	RTMControlCenter.Init(RTMConfig config);

### Create

	RTMClient client = new RTMClient(string endpoint, long pid, long uid, IRTMQuestProcessor serverPushProcessor);

Please get your project params from RTM Console.

### Configure



/////--- TODO
* Basic configs

		client.SetAutoReconnect(autoReconnect bool) 
		client.SetConnectTimeOut(timeout time.Duration)
		client.SetQuestTimeOut(timeout time.Duration)
		client.SetLogger(logger *log.Logger)

	**Note**  
	**autoReconnect** means establishing the connection in implicit or explicit. NOT keep the connection.

* Set message monitor

		client.SetMonitor(monitor RTMServerMonitor)

* Set connection events' callbacks

		client.SetOnConnectedCallback(onConnected func(connId uint64))
		client.SetOnClosedCallback(onClosed func(connId uint64))

* Config encrypted connection
	
		client.EnableEncryptor(pemKeyPath string)
		client.EnableEncryptor(pemKeyData []byte)

	RTM Server-End Go SDK using **ECC**/**ECDH** to exchange the secret key, and using **AES-128** or **AES-256** in **CFB** mode to encrypt the whole session in **stream** way.


### Connect/Dial (Optional)

	client.Connect()
	client.Dial()

Call one of these methods to do an explicit connecting action.  
If client.SetAutoReconnect(false) is called, one of these explicit connecting methods MUST be called; otherwise, these methods are optional.

### Send messages

* Send P2P Message

		mtime, err := client.SendMessage(fromUid int64, toUid int64, mtype int8, message string)
		mtime, err := client.SendMessage(fromUid int64, toUid int64, mtype int8, message string, timeout time.Duration)

		_, err := client.SendMessage(fromUid int64, toUid int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string))
		_, err := client.SendMessage(fromUid int64, toUid int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string), timeout time.Duration)

* Send Multi-Receivers P2P Message

		mtime, err := client.SendMessages(fromUid int64, toUids []int64, mtype int8, message string)
		mtime, err := client.SendMessages(fromUid int64, toUids []int64, mtype int8, message string, timeout time.Duration)

		_, err := client.SendMessages(fromUid int64, toUids []int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string))
		_, err := client.SendMessages(fromUid int64, toUids []int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string), timeout time.Duration)

* Send Group Message
	
		mtime, err := client.SendGroupMessage(fromUid int64, groupId int64, mtype int8, message string)
		mtime, err := client.SendGroupMessage(fromUid int64, groupId int64, mtype int8, message string, timeout time.Duration)

		_, err := client.SendGroupMessage(fromUid int64, groupId int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string))
		_, err := client.SendGroupMessage(fromUid int64, groupId int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string), timeout time.Duration)

* Send Room Message

		mtime, err := client.SendRoomMessage(fromUid int64, roomId int64, mtype int8, message string)
		mtime, err := client.SendRoomMessage(fromUid int64, roomId int64, mtype int8, message string, timeout time.Duration)

		_, err := client.SendRoomMessage(fromUid int64, roomId int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string))
		_, err := client.SendRoomMessage(fromUid int64, roomId int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string), timeout time.Duration)

* Send Boradcast Message

		mtime, err := client.SendBoradcastMessage(fromUid int64, mtype int8, message string)
		mtime, err := client.SendBoradcastMessage(fromUid int64, mtype int8, message string, timeout time.Duration)

		_, err := client.SendBoradcastMessage(fromUid int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string))
		_, err := client.SendBoradcastMessage(fromUid int64, mtype int8, message string, callback func (mtime int64, errorCode int, errInfo string), timeout time.Duration)


### SDK Version

	fmt.Println("RTM Server-End Go SDK Version:", rtm.SDKVersion)

## API docs

Please refer: [API docs](doc/API.md)


## Directory structure

* **<rtm-server-sdk-go>/src**

	Codes of SDK.

* **<rtm-server-sdk-go>/example**

	Examples codes for using this SDK.

* **<rtm-server-sdk-go>/doc**

	API documents in markdown format.