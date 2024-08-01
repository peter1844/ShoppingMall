Vue.component('admin', {
    template: `
        <div class="admin">
            <div>
                <input v-if="insertPermission" type="button" class="btn insert" :value="$t('common.insert')" @click="OpenInsert()"/>
            </div>
            <br/>
            <table>
                <thead>
                    <tr>
                        <th class="sort" @click="SortBy('Name')">{{ $t('admin.page.name') }}</th>
                        <th class="sort" @click="SortBy('Acc')">{{  $t('admin.page.acc') }}</th>
                        <th>{{  $t('common.operate') }}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in sortedItems" :key="item.Id">
                        <td>{{ item.Name }}</td>
                        <td>{{ item.Acc }}</td>
                        <td>
                            <input v-if="updatePermission" type="button" class="btn update" :value="$t('common.update')" @click="OpenUpdate(item.Id)"/>
                            <input v-if="deletePermission" type="button" class="btn delete" :value="$t('common.delete')" @click="DeleteAdmin(item.Id)"/>
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
                        <label><label class="required_mark">*</label>{{ $t('admin.page.name') }}</label><br><br>
                        <input type="text" class="text" maxlength="20" v-model="adminName"><br><br>

                        <label><label class="required_mark">*</label>{{ $t('admin.page.acc') }}</label><br><br>
                        <input type="text" class="text" maxlength="16" :disabled="accDisabled" v-model="acc"><br><br>

                        <label><label class="required_mark" v-if="pwdRequired">*</label>{{ $t('admin.page.pwd') }}</label><br><br>
                        <input type="text" class="text" maxlength="16" :placeholder="showPlaceholder ? this.$t('admin.message.passwordNoChange') : ''" v-model="pwd"><br><br>

                        <label><label class="required_mark">*</label>{{ $t('admin.page.role') }}</label><br><br>
                        <label v-for="items in optionData" :key="items.RoleId">
                            <input type="checkbox" :value="items.RoleId" v-model="roles"/> {{ $t('admin.option.' + items.RoleName) }}
                            <br/>
                        </label>
                        <br/><br/>

                        <label><label class="required_mark">*</label>{{ $t('admin.page.enabled') }}</label><br/>
                        <select class="select" v-model="enabled">
                            <option value="1">{{ $t('common.vaild') }}</option>
                            <option value="0">{{ $t('common.inVaild') }}</option>
                        </select>
                        <br/><br/>
                    </div>

                    <div align="right">
                        <input type="button" class="btn submit" :value="$t('common.submit')" @click="CheckAction()"/>
                        <input type="button" class="btn cancel" :value="$t('common.scancel')" @click="ClosePopup()"/>
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
            accDisabled: false,
            pwdRequired: true,
            actionType: '',
            sortKey: '',
            sortDesc: false,
            actionText: '',
            showPlaceholder: false,
            originAdminData: {},
            insertPermission: false,
            updatePermission: false,
            deletePermission: false
        }
    },
    created: function () {
        this.GetOrderPermissionData();
        this.GetAdminData();
        this.GetOptionData();
    },
    mounted() {
        window.addEventListener('keydown', this.HandleKeyDown);
    },
    methods: {
        async GetOrderPermissionData() {
            await fetch('/api/admin/getAdminPermissions', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    this.insertPermission = myJson[0].InsertPermission;
                    this.updatePermission = myJson[0].UpdatePermission;
                    this.deletePermission = myJson[0].DeletePermission;
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
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
        async GetAdminData() {
            await fetch('/api/admin/getAdminData', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    let data = myJson.reduce((obj, item) => {
                        item.Role = item.Role.map(role => role.RoleId);
                        obj[item.Id] = item;
                        return obj;
                    }, {});

                    this.adminData = Object.keys(data).map(key => data[key]);
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
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
        async GetOptionData() {
            await fetch('/api/admin/getAdminOptionData', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    this.optionData = myJson;
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
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
        async InsertAdmin() {

            if (this.acc == '' || this.pwd == '' || this.adminName == '' || this.roles.length == 0) {
                Swal.fire({
                    text: this.$t('admin.message.requiredYet'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                });

                return false;
            }

            const validCharacters = /^[a-zA-Z0-9]+$/;
            if (!validCharacters.test(this.acc) || !validCharacters.test(this.pwd)) {
                Swal.fire({
                    text: this.$t('admin.message.accOrPwdSpecialChar'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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
                if (myJson.ErrorMessage === undefined) {
                    Swal.fire({
                        text: this.$t('common.insertCompleted'),
                        icon: "success",
                        confirmButtonText: this.$t('common.confirm')
                    }).then((result) => {
                        location.reload();
                    });
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
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
        async UpdateAdmin() {

            if (this.adminName == '' || this.roles.length == 0) {
                Swal.fire({
                    text: this.$t('admin.message.requiredYet'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                });

                return false;
            }

            const validCharacters = /^[a-zA-Z0-9]+$/;
            if (!validCharacters.test(this.acc) || (this.pwd != '' && !validCharacters.test(this.pwd) )) {
                Swal.fire({
                    text: this.$t('admin.message.accOrPwdSpecialChar'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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

            let noChangeFlag = this.EditDataCheck();

            if (noChangeFlag) {
                this.showPopup = false;

                Swal.fire({
                    text: this.$t('common.noChangeData'),
                    icon: "success",
                    confirmButtonText: this.$t('common.confirm')
                })
            } else {
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
                    if (myJson.ErrorMessage === undefined) {
                        Swal.fire({
                            text: this.$t('common.updateCompleted'),
                            icon: "success",
                            confirmButtonText: this.$t('common.confirm')
                        }).then((result) => {
                            location.reload();
                        });
                    } else if (myJson.ErrorMessage == 'InvaildToken') {
                        Swal.fire({
                            text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                            icon: "error",
                            confirmButtonText: this.$t('common.confirm')
                        }).then((result) => {
                            window.location.href = '/Views/Login.aspx';
                        });
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
        DeleteAdmin(id) {
            let deleteData = this.adminData.find(item => item.Id === id);

            Swal.fire({
                html: this.$t('admin.message.deleteAlert') + '<label style="color:red;">' + deleteData.Acc + '</label>' + this.$t('admin.message.deleteAlert2'),
                icon: "question",
                confirmButtonText: this.$t('common.confirm'),
                showCancelButton: true,
                cancelButtonText: this.$t('common.cancelNoSpace'),
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
                        if (myJson.ErrorMessage === undefined) {
                            Swal.fire({
                                text: this.$t('common.deleteCompleted'),
                                icon: "success",
                                confirmButtonText: this.$t('common.confirm')
                            }).then((result) => {
                                location.reload();
                            });
                        } else if (myJson.ErrorMessage == 'InvaildToken') {
                            Swal.fire({
                                text: this.$t('common.backendMessage.' + myJson.ErrorMessage),
                                icon: "error",
                                confirmButtonText: this.$t('common.confirm')
                            }).then((result) => {
                                window.location.href = '/Views/Login.aspx';
                            });
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
            });
        },
        OpenInsert() {
            this.showPopup = true;
            this.accDisabled = false;
            this.adminId = 0;
            this.adminName = '';
            this.acc = '';
            this.pwd = '';
            this.roles = [2];
            this.enabled = 1;
            this.actionType = 'insert';
            this.pwdRequired = true;
            this.actionText = this.$t('common.insert');
            this.showPlaceholder = false;
        },
        OpenUpdate(id) {
            let updateData = this.adminData.find(item => item.Id === id);

            this.showPopup = true;
            this.accDisabled = true;
            this.adminId = id;
            this.adminName = updateData.Name;
            this.acc = updateData.Acc;
            this.pwd = '';
            this.roles = updateData.Role;
            this.enabled = updateData.Enabled;
            this.actionType = 'update';
            this.pwdRequired = false;
            this.actionText = this.$t('common.update');
            this.showPlaceholder = true;

            this.originAdminData = {
                Name: updateData.Name,
                Roles: updateData.Role,
                Enabled: updateData.Enabled
            };
        },
        EditDataCheck() {
            const nowData = {
                Name: this.adminName,
                Roles: this.roles,
                Enabled: this.enabled
            };

            if (JSON.stringify(this.originAdminData) === JSON.stringify(nowData)) {

                if (this.pwd == '') {
                    return true;
                } else {
                    return false;
                }

            } else {
                return false;
            }
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