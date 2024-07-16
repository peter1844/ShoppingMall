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
                        <th class="sort" @click="SortBy('Name')">名字</th>
                        <th class="sort" @click="SortBy('Acc')">帳號</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in sortedItems" :key="item.Id">
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
                    <h5>{{ actionText }}</h5>
                </div>

                <div class="popup_data">
                    <div>
                        <label><label style="color:red;">*</label>名字</label><br><br>
                        <input type="text" class="text" maxlength="20" v-model="adminName"><br><br>

                        <label><label style="color:red;">*</label>帳號</label><br><br>
                        <input type="text" class="text" maxlength="16" :readonly="accReadonly" v-model="acc"><br><br>

                        <label><label style="color:red;" v-if="pwdRequired">*</label>密碼</label><br><br>
                        <input type="text" class="text" maxlength="16" :placeholder="showPlaceholder ? '未輸入則不修改密碼' : ''" v-model="pwd"><br><br>

                        <label><label style="color:red;">*</label>角色</label><br><br>
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
            pwdRequired: true,
            actionType: '',
            sortKey: '',
            sortDesc: false,
            actionText: '',
            showPlaceholder: false
        }
    },
    created: function () {
        this.GetAdminData();
        this.GetOptionData();
    },
    mounted() {
        window.addEventListener('keydown', this.HandleKeyDown);
    },
    methods: {
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
                    let data = myJson.reduce((obj, item) => {
                        item.Role = item.Role.map(role => role.RoleId);
                        obj[item.Id] = item;
                        return obj;
                    }, {});

                    this.adminData = Object.keys(data).map(key => data[key]);
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
        async InsertAdmin() {

            if (this.acc == '' || this.pwd == '' || this.adminName == '' || this.roles.length == 0) {
                Swal.fire({
                    text: '尚有必填欄位未填',
                    icon: "error",
                    confirmButtonText: '確認'
                });

                return false;
            }

            const validCharacters = /^[a-zA-Z0-9]+$/;
            if (!validCharacters.test(this.acc) || !validCharacters.test(this.pwd)) {
                Swal.fire({
                    text: '帳號或密碼不得有特殊字元',
                    icon: "error",
                    confirmButtonText: '確認'
                })

                return false;
            }

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
                Swal.fire({
                    text: '系統異常，請稍後再試',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        },
        async UpdateAdmin() {

            if (this.adminName == '' || this.roles.length == 0) {
                Swal.fire({
                    text: '尚有必填欄位未填',
                    icon: "error",
                    confirmButtonText: '確認'
                });

                return false;
            }

            const validCharacters = /^[a-zA-Z0-9]+$/;
            if (!validCharacters.test(this.acc) || (this.pwd != '' && !validCharacters.test(this.pwd) )) {
                Swal.fire({
                    text: '帳號或密碼不得有特殊字元',
                    icon: "error",
                    confirmButtonText: '確認'
                })

                return false;
            }

            const data = {
                AdminId: this.adminId,
                Name: this.adminName,
                Pwd: this.pwd,
                Roles: this.roles,
                Enabled: this.enabled
            };

            await fetch('/api/admin/updateAdminData', {
                method: 'PUT',
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
                        text: '編輯完成',
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
                Swal.fire({
                    text: '系統異常，請稍後再試',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        },
        DeleteAdmin(id) {
            let deleteData = this.adminData.find(item => item.Id === id);

            Swal.fire({
                html: '確定要刪除帳號 <label style="color:red;">' + deleteData.Acc + '</label> 嗎？',
                icon: "question",
                confirmButtonText: '確認',
                showCancelButton: true,
                cancelButtonText: '取消',
            }).then((result) => {
                if (result.isConfirmed) {

                    const data = {
                        AdminId: id
                    };

                    fetch('/api/admin/deleteAdminData', {
                        method: 'DELETE',
                        headers: {
                            'token': localStorage.getItem('token'),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(data)
                    }).then((response) => {
                        return response.json()
                    }).then((myJson) => {
                        if (myJson.StatusErrorCode === undefined) {
                            Swal.fire({
                                text: '刪除完成',
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
                        Swal.fire({
                            text: '系統異常，請稍後再試',
                            icon: "error",
                            confirmButtonText: '確認'
                        })
                    })                    
                }
            });
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
            this.pwdRequired = true;
            this.actionText = '新 增';
            this.showPlaceholder = false;
        },
        OpenUpdate(id) {
            let updateData = this.adminData.find(item => item.Id === id);

            this.showPopup = true;
            this.accReadonly = true;
            this.adminId = id;
            this.adminName = updateData.Name;
            this.acc = updateData.Acc;
            this.pwd = '';
            this.roles = updateData.Role;
            this.enabled = updateData.Enabled;
            this.actionType = 'update';
            this.pwdRequired = false;
            this.actionText = '編 輯';
            this.showPlaceholder = true;
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
        },
        SortBy(key) {
            if (key === this.sortKey) {
                this.sortDesc = !this.sortDesc;
            } else {
                this.sortKey = key;
                this.sortDesc = false;
            }
        }
    },
    computed: {
        sortedItems() {
            if (this.sortKey) {
                let key = this.sortKey;
                let order = this.sortDesc ? -1 : 1;

                return this.adminData.slice().sort((a, b) => {
                    if (typeof a[key] === 'number' && typeof b[key] === 'number') {
                        return (a[key] - b[key]) * order;
                    } else {
                        return (a[key].toString().localeCompare(b[key].toString())) * order;
                    }
                });
            } else {
                return this.adminData;
            }
        }
    },
    beforeDestroy() {
        window.removeEventListener('keydown', this.HandleKeyDown);
    },
});