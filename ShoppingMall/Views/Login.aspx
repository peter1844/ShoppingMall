<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ShoppingMall.Views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>後臺管理系統</title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/vue-i18n.js" type="text/javascript"></script>
    <script src="/js/lang/Tw.js" type="text/javascript"></script>
    <script src="/js/lang/En.js" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>

    <script src="/js/pages/LoginComponent.js" type="text/javascript"></script>

    <link href="/css/pages/login.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <login></login>
        <%--<div>{{ $t('101') }}</div>--%>
    </div>
</body>
</html>

<script>
    const i18n = new VueI18n({
        locale: localStorage.getItem("lang") ?? 'tw',
        messages: {
            en: en,
            tw: tw
        }
    });

    new Vue({
        el: '#app',
        i18n
    });
</script>