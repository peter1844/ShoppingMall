Vue.component('menulist', {
    template: `
        <aside class="sidebar">
            <h4>{{welcomeText}}</h4><hr/>

            <ul class="menu">
                <li onclick="location.href='#1'">會員管理</li>
                <li onclick="location.href='#2'">商品管理</li>
                <li onclick="location.href='#3'">訂單管理</li>
                <li onclick="location.href='/Views/Admin.aspx'">後臺帳號管理</li>
                <li @click="logout">登出</li>
            </ul>
        </aside>
    `,
    data() {
        return {
            welcomeText: '你好, ' + localStorage.getItem('adminName')
        }
    },
    created: function () {

        // 馬上檢查登入是否還有效
        this.checkLoginStatus();

        // 每30秒檢查一次登入狀態
        setInterval(() => {
            this.checkLoginStatus();
        }, 10000);
    },
    methods: {
        async checkLoginStatus() {

            await fetch('/api/login/checkLoginByToken', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                if (response.status !== 200) throw new Error(response.status)
                return response.json()
            }).then((myJson) => {
                console.log('Is loging')
            }).catch((error) => {
                Swal.fire({
                    text: '登入無效或過期，請重新登入',
                    icon: "error",
                    confirmButtonText: '確認'
                }).then((result) => {
                    window.location.href = '/Views/Login.aspx';
                });
            })
        },

        logout() {
            localStorage.removeItem('adminName');
            localStorage.removeItem('token');

            window.location.href = '/Views/Login.aspx';
        }
    },
});