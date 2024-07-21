using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using TonSdk.Connect;
using TonSdk.Core;
using TonSdk.Core.Block;
using TonSdk.Core.Crypto;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Zenject;
using Message = TonSdk.Connect.Message;
using UnityUI = UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Tooltip("Toggle if you want to use presaved wallet icons. (recommended)")]
    public bool UseSavedWalletIcons = true;
    [Tooltip("Wallet icons. Works only if UseSavedWalletIcons is enabled.")]
    public List<Sprite> WalletIcons = new ();
    private List<string> WalletsIconsList = new () {"tonkeeper", "tonhub", "openmask", "dewallet", "mytonwallet", "tonflow", "tonwallet", "xtonwallet", "telegram-wallet"};

    [Header("UI References")]
    [SerializeField] private UIDocument document;
    [SerializeField] private VisualTreeAsset walletItem;

    [Header("References")]
    [Inject] private TonConnectHandler tonConnectHandler;

    [Header("Unity UI")]
    [SerializeField] private UnityUI.Button _connectWalletButton;
    [SerializeField] private UnityUI.Button _infoWalletButton;
    [SerializeField] private TextMeshProUGUI _infoWalletText;
    [SerializeField] private UnityUI.Button _disconnectWalletButton;
    [SerializeField] private UnityUI.Button _transactionButton;

    [SerializeField] private GameObject _walletButtonsView;
    [SerializeField] private WalletButtonView _walletButtonView;
    [SerializeField] private Transform _walletButtonsHolder;
    [SerializeField] private UnityUI.Button _closeButton;

    [SerializeField] private GameObject _qrCodeView;
    [SerializeField] private UnityUI.RawImage _qrRawImage;
    [SerializeField] private UnityUI.Button _qrConnectButton;
    [SerializeField] private UnityUI.Button _qrBackButton;
    [SerializeField] private TextMeshProUGUI _qrButtonText;

    private List<WalletButtonView> _walletButtons = new List<WalletButtonView>();

    private void Awake()
    {
        TonConnectHandler.OnProviderStatusChanged += OnProviderStatusChange;
        TonConnectHandler.OnProviderStatusChangedError += OnProviderStatusChangeError;
        DisableSendTXModal();
        DisableWalletInfoButton();
        EnableConnectWalletButton();

        if (tonConnectHandler.tonConnect.IsConnected)
            EnableConnectedState(tonConnectHandler.tonConnect.Wallet);

    }

    private void OnProviderStatusChange(Wallet wallet)
    {
        if(tonConnectHandler.tonConnect.IsConnected)
        {
            EnableConnectedState(wallet);
        }
        else
        {
            EnableConnectWalletButton();
            DisableWalletInfoButton();
        }
    }

    private void EnableConnectedState(Wallet wallet)
    {
        Debug.Log("Wallet connected. Address: " + wallet.Account.Address + ". Platform: " + wallet.Device.Platform + "," + wallet.Device.AppName + "," + wallet.Device.AppVersion);
        CloseConnectModal();
        DisableConnectWalletButton();
        EnableWalletInfoButton(ProcessWalletAddress(wallet.Account.Address.ToString(AddressType.Base64)));
    }

    private void OnProviderStatusChangeError(string message)
    {

    }

    #region Utilities

    private string ProcessWalletAddress(string address)
    {
        if (address.Length < 8) return address;

        string firstFourChars = address[..4];
        string lastFourChars = address[^4..];

        return firstFourChars + "..." + lastFourChars;
    }

    #endregion

    #region Button Click Events
    private async void OpenWalletQRContent(ClickEvent clickEvent, WalletConfig config)
    {
        document.rootVisualElement.Q<Label>("ConnectModal_Title").text = "Connect Wallet";

        document.rootVisualElement.Q<VisualElement>("ModalContent").style.display = DisplayStyle.None;
        document.rootVisualElement.Q<VisualElement>("ModalContentWeb").style.display = DisplayStyle.None;
        document.rootVisualElement.Q<VisualElement>("ModalQRContent").style.display = DisplayStyle.Flex;
        document.rootVisualElement.Q<VisualElement>("Button_Back").style.display = DisplayStyle.Flex;

        document.rootVisualElement.Q<Label>("ModalQRContent_OpenWalletButton_Title").text = $"Open {config.Name}";

        string connectUrl = await tonConnectHandler.tonConnect.Connect(config);
        Texture2D qrCodeTexture = QRGenerator.EncodeString(connectUrl.ToString());

        document.rootVisualElement.Q<VisualElement>("ModalQRContent_QRHandler").style.backgroundImage = new StyleBackground(qrCodeTexture);
        document.rootVisualElement.Q<VisualElement>("ModalQRContent_OpenWalletButton").UnregisterCallback<ClickEvent, string>(OpenWalletUrl);
        document.rootVisualElement.Q<VisualElement>("ModalQRContent_OpenWalletButton").RegisterCallback<ClickEvent, string>(OpenWalletUrl, connectUrl);
    }

    private async void OpenWalletQRContent(WalletConfig config)
    {
        //document.rootVisualElement.Q<Label>("ConnectModal_Title").text = "Connect Wallet";

        //document.rootVisualElement.Q<VisualElement>("ModalContent").style.display = DisplayStyle.None;
        //document.rootVisualElement.Q<VisualElement>("ModalContentWeb").style.display = DisplayStyle.None;
        //document.rootVisualElement.Q<VisualElement>("ModalQRContent").style.display = DisplayStyle.Flex;
        //document.rootVisualElement.Q<VisualElement>("Button_Back").style.display = DisplayStyle.Flex;

        _qrCodeView.SetActive(true);
        _walletButtonsView.SetActive(false);

        //document.rootVisualElement.Q<Label>("ModalQRContent_OpenWalletButton_Title").text = $"Open {config.Name}";
        _qrButtonText.SetText($"Open {config.Name}");

        string connectUrl = await tonConnectHandler.tonConnect.Connect(config);
        
        Texture2D qrCodeTexture = QRGenerator.EncodeString(connectUrl.ToString());

        //document.rootVisualElement.Q<VisualElement>("ModalQRContent_QRHandler").style.backgroundImage = new StyleBackground(qrCodeTexture);
        _qrRawImage.texture = qrCodeTexture;
        //document.rootVisualElement.Q<VisualElement>("ModalQRContent_OpenWalletButton").UnregisterCallback<ClickEvent, string>(OpenWalletUrl);
        //document.rootVisualElement.Q<VisualElement>("ModalQRContent_OpenWalletButton").RegisterCallback<ClickEvent, string>(OpenWalletUrl, connectUrl);
        _qrConnectButton.onClick.RemoveListener(() => OpenWalletUrl(connectUrl));
        _qrConnectButton.onClick.AddListener(() => OpenWalletUrl(connectUrl));
    }

    private async void OpenWebWallet(ClickEvent clickEvent, WalletConfig config)
    {
        await tonConnectHandler.tonConnect.Connect(config);
    }

    private void OpenWalletUrl(ClickEvent clickEvent, string url)
    {
        OpenWalletUrl(url);
    }

    private void OpenWalletUrl(string url)
    {
        string escapedUrl = Uri.EscapeUriString(url);
        Debug.Log($"Connecting {escapedUrl}");
        Application.OpenURL(escapedUrl);
    }

    private void CloseConnectModal(ClickEvent clickEvent) => CloseConnectModal();

    private async void SendTestTransaction()
    {
        Address receiver = new("UQBCM9qKwEww-9Xj3FZD9w_RqjlwcJ4hKvrV7uMDtnUmjTEA");
        Coins amount = new(0.001);
        Message[] sendTons =
        {
            new Message(receiver, amount),
            //new Message(receiver, amount),
            //new Message(receiver, amount),
            //new Message(receiver, amount),
        };

        long validUntil = DateTimeOffset.Now.ToUnixTimeSeconds() + 600;

        SendTransactionRequest transactionRequest = new SendTransactionRequest(sendTons, validUntil, CHAIN.MAINNET, tonConnectHandler.tonConnect.Account.Address);

        Debug.Log("Account");
        Debug.Log(tonConnectHandler.tonConnect.Account.Address.ToBOC());
        Debug.Log(tonConnectHandler.tonConnect.Account.Address.ToString());
        Debug.Log(tonConnectHandler.tonConnect.Account.PublicKey);
        Debug.Log(tonConnectHandler.tonConnect.Account.WalletStateInit);

        Debug.Log("Wallet");
        Debug.Log(tonConnectHandler.tonConnect.Wallet.Device.AppName);
        Debug.Log(tonConnectHandler.tonConnect.Wallet.Device.AppVersion);
        Debug.Log(tonConnectHandler.tonConnect.Wallet.Device.Platform);
        Debug.Log(tonConnectHandler.tonConnect.Wallet.Device.MaxProtocolVersion);

        Debug.Log(tonConnectHandler.tonConnect.Wallet.Provider);

        //OpenWalletUrl(tonConnectHandler.tonConnect.Account.WalletStateInit);
        //Application.OpenURL(tonConnectHandler.CurrentConnectedWallet.UniversalUrl);
        //await tonConnectHandler.SendTransaction("UQBCM9qKwEww-9Xj3FZD9w_RqjlwcJ4hKvrV7uMDtnUmjTEA", (double)0.001);
        tonConnectHandler.OnTransactionSendingFinish += OnTransactionSendingFinish;

        await tonConnectHandler.SendTransaction("UQBCM9qKwEww-9Xj3FZD9w_RqjlwcJ4hKvrV7uMDtnUmjTEA", 0.001);
        //if (result.HasValue)
        //    Debug.Log(result.Value.Boc);
    }

    private void OnTransactionSendingFinish(SendTransactionResult? sendTransaction, bool isSuccess)
    {
        tonConnectHandler.OnTransactionSendingFinish -= OnTransactionSendingFinish;

        if (isSuccess && sendTransaction.HasValue)
        {
            Debug.Log("Transaction successfull");
        }
        else
            Debug.Log($"Transaction failed");
    }

    private void BackToMainContent(ClickEvent clickEvent)
    {
        tonConnectHandler.tonConnect.PauseConnection();

        document.rootVisualElement.Q<Label>("ConnectModal_Title").text = "Choose Wallet";
        document.rootVisualElement.Q<VisualElement>("ModalContent").style.display = DisplayStyle.Flex;
        document.rootVisualElement.Q<VisualElement>("ModalContentWeb").style.display = DisplayStyle.Flex;
        document.rootVisualElement.Q<VisualElement>("ModalQRContent").style.display = DisplayStyle.None;
        document.rootVisualElement.Q<VisualElement>("Button_Back").style.display = DisplayStyle.None;
    }

    private void BackToMainContent()
    {
        tonConnectHandler.tonConnect.PauseConnection();

        //document.rootVisualElement.Q<Label>("ConnectModal_Title").text = "Choose Wallet";
        //document.rootVisualElement.Q<VisualElement>("ModalContent").style.display = DisplayStyle.Flex;
        //document.rootVisualElement.Q<VisualElement>("ModalContentWeb").style.display = DisplayStyle.Flex;
        //document.rootVisualElement.Q<VisualElement>("ModalQRContent").style.display = DisplayStyle.None;
        //document.rootVisualElement.Q<VisualElement>("Button_Back").style.display = DisplayStyle.None;

        _qrCodeView.SetActive(false);
        _walletButtonsView.SetActive(true);
    }

    private void ConnectWalletButtonClick(ClickEvent clickEvent)
    {
        ConnectWalletButtonClick();
    }
    private void ConnectWalletButtonClick()
    {
        ShowConnectModal();
    }

    private void DisconnectWalletButtonClick(ClickEvent clickEvent)
    {
        DisconnectWalletButtonClick();
    }

    private async void DisconnectWalletButtonClick()
    {
        EnableConnectWalletButton();
        DisableWalletInfoButton();
        tonConnectHandler.RestoreConnectionOnAwake = false;
        await tonConnectHandler.tonConnect.Disconnect();
    }

    private  void WalletInfoButtonClick(ClickEvent clickEvent)
    {
        WalletInfoButtonClick();
    }

    private void WalletInfoButtonClick()
    {
        ShowSendTXModal();
    }

    private void CloseTXModalButtonClick(ClickEvent clickEvent)
    {
        DisableSendTXModal();
    }

    private async void SendTXModalSendButtonClick(ClickEvent clickEvent)
    {
        string receiverAddress = document.rootVisualElement.Q<TextField>("SendTXModal_Address").value;
        double sendValue = document.rootVisualElement.Q<DoubleField>("SendTXModal_Value").value;
        if(string.IsNullOrEmpty(receiverAddress) || sendValue <= 0) return;

        Address receiver = new(receiverAddress);
        Coins amount = new(sendValue);
        Message[] sendTons = 
        {
            new Message(receiver, amount),
            //new Message(receiver, amount),
            //new Message(receiver, amount),
            //new Message(receiver, amount),
        };

        long validUntil = DateTimeOffset.Now.ToUnixTimeSeconds() + 600;

        SendTransactionRequest transactionRequest = new SendTransactionRequest(sendTons, validUntil);
        await tonConnectHandler.tonConnect.SendTransaction(transactionRequest);
    }

    #endregion

    #region Tasks
    private void LoadWalletsCallback(List<WalletConfig> wallets)
    {
        // Here you can do something with the wallets list
        // for example: add them to the connect modal window
        // Warning! Use coroutines to load data from the web
        StartCoroutine(LoadWalletsIntoModal(wallets));
    }

    private IEnumerator LoadWalletsIntoModal(List<WalletConfig> wallets)
    {
        //VisualElement contentElement = document.rootVisualElement.Q<VisualElement>("ModalContent");
        //VisualElement jsContentElement = document.rootVisualElement.Q<VisualElement>("ModalContentWeb");
        //document.rootVisualElement.Q<Label>("ConnectModal_Title").text = "Choose Wallet";
        //contentElement.Clear();
        //jsContentElement.Clear();
        //contentElement.style.display = DisplayStyle.None;
        //jsContentElement.style.display = DisplayStyle.None;
        if (wallets.Count == 0) yield break;

        ClearButtons();

        // load http bridge wallets
        foreach (var t in wallets)
        {
            if(t.BridgeUrl == null) continue;
            //VisualElement walletElement = walletItem.CloneTree();
            var walletButton = Instantiate(_walletButtonView, _walletButtonsHolder);

            if(UseSavedWalletIcons && WalletsIconsList.Contains(t.AppName))
            {
                walletButton.WalletRawImage.gameObject.SetActive(false);
                walletButton.WalletImage.sprite = WalletIcons[WalletsIconsList.IndexOf(t.AppName)];
                walletButton.WalletImage.gameObject.SetActive(true);
                //walletElement.Q<VisualElement>("WalletButton_WalletImage").style.backgroundImage = new StyleBackground(WalletIcons[WalletsIconsList.IndexOf(t.AppName)]);
            }
            else
            {
                using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(t.Image))
                {
                    yield return request.SendWebRequest();

                    if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) 
                        Debug.LogError("Error while loading wallet image: " + request.error);
                    else
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(request);
                        if (texture != null)
                        {
                            walletButton.WalletImage.gameObject.SetActive(false);
                            walletButton.WalletRawImage.texture = texture;
                            walletButton.WalletRawImage.gameObject.SetActive(true);
                            //walletButton.WalletImage.sprite = texture as Sprite;
                            //walletElement.Q<VisualElement>("WalletButton_WalletImage").style.backgroundImage = new StyleBackground(texture);
                        }
                    }
                }
            }

            //walletElement.Q<Label>("WalletButton_WalletName").text = t.Name;
            //walletElement.RegisterCallback<ClickEvent, WalletConfig>(OpenWalletQRContent, t);

            walletButton.WalletName.SetText(t.Name);
            walletButton.WalletButton.onClick.AddListener(() => { OpenWalletQRContent(t); });

            _walletButtons.Add(walletButton);

            //contentElement.Add(walletElement);
        }

        // load js bridge wallets
        if(tonConnectHandler.UseWebWallets)
        {
            for (int i = 0; i < wallets.Count; i++)
            {
                if(wallets[i].JsBridgeKey == null || !InjectedProvider.IsWalletInjected(wallets[i].JsBridgeKey)) continue;
                //VisualElement walletElement = walletItem.CloneTree();
                var walletButton = Instantiate(_walletButtonView, _walletButtonsHolder);

                if (UseSavedWalletIcons && WalletsIconsList.Contains(wallets[i].AppName))
                {
                    walletButton.WalletRawImage.gameObject.SetActive(false);
                    walletButton.WalletImage.sprite = WalletIcons[WalletsIconsList.IndexOf(wallets[i].AppName)];
                    walletButton.WalletImage.gameObject.SetActive(true);
                    //walletElement.Q<VisualElement>("WalletButton_WalletImage").style.backgroundImage = new StyleBackground(WalletIcons[WalletsIconsList.IndexOf(wallets[i].AppName)]);
                }
                else
                {
                    using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(wallets[i].Image))
                    {
                        yield return request.SendWebRequest();

                        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError) 
                            Debug.LogError("Error while loading wallet image: " + request.error);
                        else
                        {
                            Texture2D texture = DownloadHandlerTexture.GetContent(request);
                            if (texture != null)
                            {
                                walletButton.WalletImage.gameObject.SetActive(false);
                                walletButton.WalletRawImage.texture = texture;
                                walletButton.WalletRawImage.gameObject.SetActive(true);
                                //walletElement.Q<VisualElement>("WalletButton_WalletImage").style.backgroundImage = new StyleBackground(texture);
                            }
                        }
                    }
                }

                //walletElement.Q<Label>("WalletButton_WalletName").text = wallets[i].Name;
                //walletElement.RegisterCallback<ClickEvent, WalletConfig>(OpenWebWallet, wallets[i]);

                walletButton.WalletName.SetText(wallets[i].Name);
                walletButton.WalletButton.onClick.AddListener(() => { OpenWalletQRContent(wallets[i]); });

                _walletButtons.Add(walletButton);

                //jsContentElement.Add(walletElement);
            }
        }

        _walletButtonsView.SetActive(true);

        //contentElement.style.display = DisplayStyle.Flex;
        //jsContentElement.style.display = DisplayStyle.Flex;
    }
    #endregion

    #region UI Methods

    public void ShowConnectModal()
    {
        if (tonConnectHandler.tonConnect.IsConnected)
        {
            Debug.LogWarning("Wallet already connected. The connection window has not been opened. Before proceeding, please disconnect from your wallet.");
            //document.rootVisualElement.Q<VisualElement>("ConnectModal").style.display = DisplayStyle.None;

            _walletButtonsView.SetActive(false);

            return;
        }
        tonConnectHandler.CreateTonConnectInstance();

        //document.rootVisualElement.Q<Label>("ConnectModal_Title").text = "Choose Wallet";

        //document.rootVisualElement.Q<VisualElement>("ModalContent").style.display = DisplayStyle.Flex;
        //document.rootVisualElement.Q<VisualElement>("ModalContentWeb").style.display = DisplayStyle.Flex;
        //document.rootVisualElement.Q<VisualElement>("ModalQRContent").style.display = DisplayStyle.None;
        //document.rootVisualElement.Q<VisualElement>("Button_Back").style.display = DisplayStyle.None;
        //document.rootVisualElement.Q<VisualElement>("ConnectModal").style.display = DisplayStyle.Flex;

        _walletButtonsView.SetActive(true);
        _qrCodeView.SetActive(false);

        //document.rootVisualElement.Q<VisualElement>("Button_Back").UnregisterCallback<ClickEvent>(BackToMainContent);
        //document.rootVisualElement.Q<VisualElement>("Button_Back").RegisterCallback<ClickEvent>(BackToMainContent);
        //document.rootVisualElement.Q<VisualElement>("Button_Close").UnregisterCallback<ClickEvent>(CloseConnectModal);
        //document.rootVisualElement.Q<VisualElement>("Button_Close").RegisterCallback<ClickEvent>(CloseConnectModal);

        _qrBackButton.onClick.RemoveListener(BackToMainContent);
        _qrBackButton.onClick.AddListener(BackToMainContent);

        _closeButton.onClick.RemoveListener(CloseConnectModal);
        _closeButton.onClick.AddListener(CloseConnectModal);

        //StartCoroutine(tonConnectHandler.LoadWallets("https://raw.githubusercontent.com/ton-blockchain/wallets-list/main/wallets-v2.json", LoadWalletsCallback));
        LoadWalletsCallback(tonConnectHandler.WalletConfigs);
    }


    private void CloseConnectModal()
    {
        if(!tonConnectHandler.tonConnect.IsConnected) tonConnectHandler.tonConnect.PauseConnection();
        //document.rootVisualElement.Q<VisualElement>("ConnectModal").style.display = DisplayStyle.None;
        _walletButtonsView?.SetActive(false);
    }

    private void EnableConnectWalletButton()
    {
        // enable connect button
        //document.rootVisualElement.Q<VisualElement>("ConnectWalletButton").UnregisterCallback<ClickEvent>(ConnectWalletButtonClick);
        //document.rootVisualElement.Q<VisualElement>("ConnectWalletButton").RegisterCallback<ClickEvent>(ConnectWalletButtonClick);
        //document.rootVisualElement.Q<VisualElement>("ConnectWalletButton").style.display = DisplayStyle.Flex;

        _connectWalletButton.gameObject.SetActive(true);
        _connectWalletButton.onClick.RemoveListener(ConnectWalletButtonClick);
        _connectWalletButton.onClick.AddListener(ConnectWalletButtonClick);
    }

    private void EnableWalletInfoButton(string wallet)
    {
        // enable wallet info and disconnect button
        //document.rootVisualElement.Q<VisualElement>("WalletInfoButton").UnregisterCallback<ClickEvent>(WalletInfoButtonClick);
        //document.rootVisualElement.Q<VisualElement>("WalletInfoButton").RegisterCallback<ClickEvent>(WalletInfoButtonClick);
        //document.rootVisualElement.Q<VisualElement>("WalletInfoButton").style.display = DisplayStyle.Flex;

        _infoWalletButton.onClick.RemoveListener(WalletInfoButtonClick);
        _infoWalletButton.onClick.AddListener(WalletInfoButtonClick);
        _infoWalletButton.gameObject.SetActive(true);

        _transactionButton.onClick.RemoveListener(SendTestTransaction);
        _transactionButton.onClick.AddListener(SendTestTransaction);

        //document.rootVisualElement.Q<VisualElement>("DisconnectWalletButton").UnregisterCallback<ClickEvent>(DisconnectWalletButtonClick);
        //document.rootVisualElement.Q<VisualElement>("DisconnectWalletButton").RegisterCallback<ClickEvent>(DisconnectWalletButtonClick);
        //document.rootVisualElement.Q<VisualElement>("DisconnectWalletButton").style.display = DisplayStyle.Flex;

        _disconnectWalletButton.onClick.RemoveListener(DisconnectWalletButtonClick);
        _disconnectWalletButton.onClick.AddListener(DisconnectWalletButtonClick);
        _disconnectWalletButton.gameObject.SetActive(true);

        //document.rootVisualElement.Q<Label>("WalletInfoButton_Title").text = wallet;
        _infoWalletText.SetText(wallet);
    }

    private void DisableConnectWalletButton()
    {
        // disable connect button
        //document.rootVisualElement.Q<VisualElement>("ConnectWalletButton").style.display = DisplayStyle.None;
        _connectWalletButton.gameObject.SetActive(false);
    }

    private void DisableWalletInfoButton()
    {
        // disable wallet info and disconnect button
        //document.rootVisualElement.Q<VisualElement>("WalletInfoButton").style.display = DisplayStyle.None;
        //document.rootVisualElement.Q<VisualElement>("DisconnectWalletButton").style.display = DisplayStyle.None;

        _infoWalletButton.gameObject.SetActive(false);
        _disconnectWalletButton.gameObject.SetActive(false);
    }

    private void ShowSendTXModal()
    {
        document.rootVisualElement.Q<VisualElement>("SendTXModal").style.display = DisplayStyle.Flex;
        document.rootVisualElement.Q<VisualElement>("SendTXModal_Button_Close").UnregisterCallback<ClickEvent>(CloseTXModalButtonClick);
        document.rootVisualElement.Q<VisualElement>("SendTXModal_Button_Close").RegisterCallback<ClickEvent>(CloseTXModalButtonClick);
        document.rootVisualElement.Q<VisualElement>("SendTXModal_ConfirmButton").UnregisterCallback<ClickEvent>(SendTXModalSendButtonClick);
        document.rootVisualElement.Q<VisualElement>("SendTXModal_ConfirmButton").RegisterCallback<ClickEvent>(SendTXModalSendButtonClick);
    }

    private void DisableSendTXModal()
    {
        //document.rootVisualElement.Q<VisualElement>("SendTXModal").style.display = DisplayStyle.None;
    }
    #endregion

    private void ClearButtons()
    {
        for (int i = 0; i < _walletButtons.Count; i++)
            Destroy(_walletButtons[i].gameObject);

        _walletButtons.Clear();
    }
}

   

  