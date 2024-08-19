Vue.component('menulist', {
    template: `
        <aside class="sidebar">
            <h4>{{ welcomeText }}</h4>
            <select class="languageSelect" v-model="language" @change="changeLanguage">
                <option value="tw">{{ $t('menu.page.tw') }}</option>
                <option value="en">{{ $t('menu.page.en') }}</option>
            </select><br/>
            <hr/>

            <ul class="menu">
                <li v-if="memberPermission" onclick="location.href='/Views/Member.aspx'">{{ $t('menu.page.member') }}</li>
                <li v-if="commodityPermission" onclick="location.href='/Views/Commodity.aspx'">{{ $t('menu.page.commodity') }}</li>
                <li v-if="orderPermission" onclick="location.href='/Views/Order.aspx'">{{ $t('menu.page.order') }}</li>
                <li v-if="adminPermission" onclick="location.href='/Views/Admin.aspx'">{{ $t('menu.page.admin') }}</li>
                <li @click="logout">{{ $t('menu.page.logout') }}</li>
            </ul>
        </aside>
    `,
    data() {
        return {
            welcomeText: this.$t('menu.page.welcomeText') + localStorage.getItem('adminName'),
            memberPermission: true,
            commodityPermission: true,
            orderPermission: true,
            adminPermission: true,
            language: globalLang,
        }
    },
    created: function () {

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
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                }

            }).catch((error) => {

                Swal.fire({
                    text: this.$t('common.systemError'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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
                            html: this.$t('menu.message.goAlert') + '<label style="color:red;">' + myJson[0].InventoryShortageCount + '</label>' + this.$t('menu.message.goAlert2'),
                            icon: "question",
                            confirmButtonText: this.$t('common.go'),
                            showCancelButton: true,
                            cancelButtonText: this.$t('common.cancelNoSpace'),
                        }).then((result) => {
                            if (result.isConfirmed) {
                                window.location.href = '/Views/Commodity.aspx';
                            }
                        });
                    }
                } else {
                    Swal.fire({
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                }

            }).catch((error) => {

                Swal.fire({
                    text: this.$t('common.systemError'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    })
                }
            }).catch((error) => {
                Swal.fire({
                    text: this.$t('common.systemError'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                })
            })
        },
        async changeLanguage() {

            const data = {
                Language: this.language
            };

            await fetch('/api/menu/setLanguage', {
                method: 'POST',
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    location.reload();
                } else {
                    Swal.fire({
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    })
                }

            }).catch((error) => {

                Swal.fire({
                    text: this.$t('common.systemError'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                })
            })            
        }
    },
});