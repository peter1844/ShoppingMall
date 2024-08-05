<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="ShoppingMall.Views.Admin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/vue-i18n.js" type="text/javascript"></script>
    <script src="<%= GetVersionUrl("lang/Tw.js", "js") %>" type="text/javascript"></script>
    <script src="<%= GetVersionUrl("lang/En.js", "js") %>" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>

    <script src="<%= GetVersionUrl("commons/MenuComponent.js", "js") %>" type="text/javascript"></script>
    <script src="<%= GetVersionUrl("pages/AdminComponent.js", "js") %>" type="text/javascript"></script>

    <link href="<%= GetVersionUrl("Commons/menu.css", "css") %>" rel="stylesheet" type="text/css"/>
    <link href="<%= GetVersionUrl("Pages/admin.css", "css") %>" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <menulist></menulist>

        <div class="main-content">
            <admin></admin>
        </div>
    </div>
</body>
</html>

<script>
    const i18n = new VueI18n({
        //locale: localStorage.getItem("lang") ?? 'tw',
        locale: '<%=Session["lang"] %>',
        messages: {
            en: en,
            tw: tw
        }
    });

    new Vue({
        el: '#app',
        i18n,
        created: function () {
            document.title = this.$t('admin.page.admin')
        }
    });
</script>
