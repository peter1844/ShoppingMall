<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Commodity.aspx.cs" Inherits="ShoppingMall.Views.Ccommodity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/vue-i18n.js" type="text/javascript"></script>
    <script src="<%= GetVersionUrl("/js/lang/Tw.js") %>" type="text/javascript"></script>
    <script src="<%= GetVersionUrl("/js/lang/En.js") %>" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>

    <script src="<%= GetVersionUrl("/js/commons/MenuComponent.js") %>" type="text/javascript"></script>
    <script src="<%= GetVersionUrl("/js/pages/CommodityComponent.js") %>" type="text/javascript"></script>

    <link href="<%= GetVersionUrl("/css/Commons/menu.css") %>" rel="stylesheet" type="text/css"/>
    <link href="<%= GetVersionUrl("/css/Pages/commodity.css") %>" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <menulist></menulist>

        <div class="main-content">
            <commodity></commodity>
        </div>
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
        i18n,
        created: function () {
            document.title = this.$t('commodity.page.commodity')
        }
    });
</script>