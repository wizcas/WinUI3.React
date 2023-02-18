import { FC, useEffect, useState } from "react";

function getWebView(): any {
  return (window as any).chrome.webview;
}

export const Interop: FC = () => {
  function sendToNative() {
    getWebView().postMessage("Hello from React");
  }

  const [nativeMessage, setNativeMessage] = useState("");

  useEffect(() => {
    function handleMessage(event: any) {
      setNativeMessage(event.data);
    }
    const webView = getWebView();
    webView.addEventListener("message", handleMessage);
    return () => {
      webView.removeEventListener("message", handleMessage);
    };
  }, []);

  return (
    <div>
      <h1>Interop</h1>
      <h3>Message from Native:</h3>
      <p>{nativeMessage}</p>
      <button onClick={sendToNative}>Send Message to Native</button>
    </div>
  );
};
