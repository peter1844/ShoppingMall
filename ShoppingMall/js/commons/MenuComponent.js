Vue.component('menulist', {
    template: `
        <aside class="sidebar">
            <h4>{{welcomeText}}</h4><hr/>

            <ul class="menu">
                <li v-if="memberPermission" onclick="location.href='/Views/Member.aspx'">會員管理</li>
                <li v-if="commodityPermission" onclick="location.href='/Views/Commodity.aspx'">商品管理</li>
                <li v-if="orderPermission" onclick="location.href='/Views/Order.aspx'">訂單管理</li>
                <li v-if="adminPermission" onclick="location.href='/Views/Admin.aspx'">後臺帳號管理</li>
                <li @click="logout">登出</li>
            </ul>
        </aside>
    `,
    data() {
        return {
            welcomeText: '你好, ' + localStorage.getItem('adminName'),
            memberPermission: false,
            commodityPermission: false,
            orderPermission: false,
            adminPermission: false,
        }
    },
    created: function () {

        this.GetMemberPermissionData();

        // 每10秒檢查一次登入狀態
        setInterval(() => {
            this.checkLoginStatus();
        }, 10000);

        if (localStorage.getItem('stockAlert') == null) {
            this.checkCommodityStock();
        } else {
            let alertData = JSON.parse(localStorage.getItem('stockAlert'));

            // 判斷庫存量警示的有效時間是否已到期
            if (new Date().getTime() >= alertData.expiration) this.checkCommodityStock();
        }
    },
    methods: {
        async GetMemberPermissionData() {
            await fetch('/api/menu/getMenuPermissions', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    this.memberPermission = myJson[0].MemberPermission;
                    this.commodityPermission = myJson[0].CommodityPermission;
                    this.orderPermission = myJson[0].OrderPermission;
                    this.adminPermission = myJson[0].AdminPermission;
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: myJson.ErrorMessage,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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
        },
        async checkLoginStatus() {

            await fetch('/api/login/checkLoginByToken', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {

                if (myJson.ErrorMessage === undefined) {
                    console.log('Is loging')
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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
        async checkCommodityStock() {

            await fetch('/api/commodity/checkCommodityStock', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {

                if (myJson.ErrorMessage === undefined) {

                    // 有庫存量不足的商品
                    if (myJson[0].InventoryShortageCount > 0) {

                        const data = { 
                            value: myJson[0].InventoryShortageCount,
                            expiration: new Date().getTime() + 10 * 60 * 1000 
                        };
                        localStorage.setItem('stockAlert', JSON.stringify(data));

                        Swal.fire({
                            html: '偵測到有 <label style="color:red;">' + myJson[0].InventoryShortageCount + '</label> 筆商品庫存量不足，要前往商品頁面嗎？',
                            icon: "question",
                            confirmButtonText: '前往',
                            showCancelButton: true,
                            cancelButtonText: '取消',
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = '/Views/Commodity.aspx';
                            }
                        });
                    }
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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

                if (myJson.ErrorMessage === undefined) {
                    localStorage.removeItem('adminName');
                    localStorage.removeItem('token');

                    window.location.href = '/Views/Login.aspx';
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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