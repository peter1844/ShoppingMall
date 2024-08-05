<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Member.aspx.cs" Inherits="ShoppingMall.Views.Member" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script src="/js/vue.js" type="text/javascript"></script>
    <script src="/js/vue-i18n.js" type="text/javascript"></script>
    <script src="<%= GetLanguagePackage() %>" type="text/javascript"></script>
    <script src="/js/SweetAlert2.js" type="text/javascript"></script>

    <script src="<%= GetVersionUrl("commons/MenuComponent.js", "js") %>" type="text/javascript"></script>
    <script src="<%= GetVersionUrl("pages/MemberComponent.js", "js") %>" type="text/javascript"></script>

    <link href="<%= GetVersionUrl("Commons/menu.css", "css") %>" rel="stylesheet" type="text/css"/>
    <link href="<%= GetVersionUrl("Pages/member.css", "css") %>" rel="stylesheet" type="text/css"/>
</head>
<body>
    <div id="app">
        <menulist></menulist>

        <div class="main-content">
            <member></member>
        </div>
    </div>
</body>
</html>

<script>
    var globalLang = '<%=Session["lang"] %>';

    const i18n = new VueI18n({
        locale: globalLang,
        messages: {
            '<%=Session["lang"] %>': <%=Session["lang"] %>,
        }
    });

    new Vue({
        el: '#app',
        i18n,
        created: function () {
            document.title = this.$t('member.page.member')
        }
    });
</script>