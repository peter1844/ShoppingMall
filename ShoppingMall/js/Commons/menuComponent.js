Vue.component('menulist', {
    template: `
        <aside class="sidebar">
            <h4>{{welcomeText}}</h4><hr/>

            <ul class="menu">
                <li onclick="location.href='/Views/Member.aspx'">會員管理</li>
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

        // 每30秒檢查一次登入狀態
        setInterval(() => {
            this.checkLoginStatus();
        }, 30000);
    },
    methods: {
        async checkLoginStatus() {

            await fetch('/api/login/checkLoginByToken', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {

                if (myJson.StatusErrorCode === undefined) {
                    console.log('Is loging')
                } else {
                    Swal.fire({
                        text: myJson.StatusErrorCode,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                }

            }).catch((error) => {

                Swal.fire({
                    text: '系統異常，請稍後再試',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        },

        async logout() {

            await fetch('/api/logout/logout', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {

                if (myJson.StatusErrorCode === undefined) {
                    localStorage.removeItem('adminName');
                    localStorage.removeItem('token');

                    window.location.href = '/Views/Login.aspx';
                } else {
                    Swal.fire({
                        text: myJson.StatusErrorCode,
                        icon: "error",
                        confirmButtonText: '確認'
                    })
                }
            }).catch((error) => {
                Swal.fire({
                    text: '系統異常，請稍後再試',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        }
    },
});