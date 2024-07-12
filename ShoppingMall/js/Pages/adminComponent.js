Vue.component('admin', {
    template: `
        <div class="admin">
            <div>
                <input type="button" class="btn insert" value="新 增" @click="OpenInsert()"/>
            </div>
            <br/>
            <table>
                <thead>
                    <tr>
                        <th>名字</th>
                        <th>帳號</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in adminData" :key="item.Id">
                        <td>{{ item.Name }}</td>
                        <td>{{ item.Acc }}</td>
                        <td>
                            <input type="button" class="btn update" value="編 輯" @click="OpenUpdate(item.Id)"/>
                            <input type="button" class="btn delete" value="刪 除" @click="DeleteAdmin(item.Id)"/>
                        </td>
                    </tr>
                </tbody>
            </table>

            <div class="popup" v-if="showPopup">
                <div class="popup_head">
                    <h5>新 增</h5>
                </div>

                <div class="popup_data">
                    <div>
                        <label>名字</label><br><br>
                        <input type="text" class="text" maxlength="20" v-model="adminName"><br><br>

                        <label>帳號</label><br><br>
                        <input type="text" class="text" maxlength="16" :readonly="accReadonly" v-model="acc"><br><br>

                        <label>密碼</label><br><br>
                        <input type="text" class="text" maxlength="16" v-model="pwd"><br><br>

                        <label>角色</label><br><br>
                        <label v-for="items in optionData" :key="items.RoleId">
                            <input type="checkbox" :value="items.RoleId" v-model="roles"/> {{ items.RoleName }}
                            <br/>
                        </label>
                        <br/><br/>

                        <label>啟用狀態</label><br/>
                        <select class="select" v-model="enabled">
                            <option value="1">有效</option>
                            <option value="0">無效</option>
                        </select>
                        <br/><br/>
                    </div>

                    <div align="right">
                        <input type="button" class="btn submit" value="送 出" @click="CheckAction()"/>
                        <input type="button" class="btn cancel" value="取 消" @click="ClosePopup()"/>
                    </div>
                </div>
            </div>

            <div class="overlay" v-if="showPopup"></div>
        </div>
    `,
    data() {
        return {
            adminData: {},
            optionData: {},
            showPopup: false,
            adminId: 0,
            adminName: '',
            acc: '',
            pwd: '',
            roles: [],
            enabled: 1,
            accReadonly: false,
            actionType: ''
        }
    },
    created: function () {
        this.GetAdminData();
        this.GetOptionData();
    },
    methods: {
        async GetOptionData() {
            await fetch('/api/admin/getAdminOptionData', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.StatusErrorCode === undefined) {
                    this.optionData = myJson;
                } else if (myJson.StatusErrorCode == 'A401') {
                    Swal.fire({
                        text: myJson.StatusErrorCode,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
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
        },
        async GetAdminData() {
            await fetch('/api/admin/getAdminData', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.StatusErrorCode === undefined) {
                    this.adminData = myJson.reduce((obj, item) => {
                        item.Role = item.Role.map(role => role.RoleId);
                        obj[item.Id] = item;
                        return obj;
                    }, {});
                } else if (myJson.StatusErrorCode == 'A401') {
                    Swal.fire({
                        text: myJson.StatusErrorCode,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.StatusErrorCode,
                        icon: "error",
                        confirmButtonText: '確認'
                    })
                }
            }).catch((error) => {
                console.log(error);
                Swal.fire({
                    text: '系統異常，請稍後再試',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        },
        async InsertAdmin() {

            const data = {
                Name: this.adminName,
                Acc: this.acc,
                Pwd: this.pwd,
                Roles: this.roles,
                Enabled: this.enabled
            };

            await fetch('/api/admin/insertAdminData', {
                method: 'POST',
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data)
            }).then((response) => {
                this.showPopup = false;

                return response.json()
            }).then((myJson) => {
                if (myJson.StatusErrorCode === undefined) {
                    Swal.fire({
                        text: '新增完成',
                        icon: "success",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        location.reload();
                    });
                } else if (myJson.StatusErrorCode == 'A401') {
                    Swal.fire({
                        text: myJson.StatusErrorCode,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.StatusErrorCode,
                        icon: "error",
                        confirmButtonText: '確認'
                    })
                }
            }).catch((error) => {
                console.log(error);
                Swal.fire({
                    text: '系統異常，請稍後再試',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        },
        async UpdateAdmin() {
            console.log('u' + this.adminId);
        },
        async DeleteAdmin(id) {
            console.log("D" + id);
        },
        OpenInsert() {
            this.showPopup = true;
            this.accReadonly = false;
            this.adminId = 0;
            this.adminName = '';
            this.acc = '';
            this.pwd = '';
            this.roles = [2];
            this.enabled = 1;
            this.actionType = 'insert';
        },
        OpenUpdate(id) {
            this.showPopup = true;
            this.accReadonly = true;
            this.adminId = id,
            this.adminName = this.adminData[id].Name;
            this.acc = this.adminData[id].Acc;
            this.pwd = '';
            this.roles = this.adminData[id].Role;
            this.enabled = this.adminData[id].Enabled;
            this.actionType = 'update';
        },
        CheckAction() {
            this.actionType == 'insert' ? this.InsertAdmin() : this.UpdateAdmin();
        },
        ClosePopup() {
            this.showPopup = false;
        },
        HandleKeyDown(event) {
            if (event.keyCode === 27) {
                this.ClosePopup();
            }
        }

    },
    mounted() {
        window.addEventListener('keydown', this.HandleKeyDown);
    },
    beforeDestroy() {
        window.removeEventListener('keydown', this.HandleKeyDown);
    },
});