Vue.component('member', {
    template: `
        <div class="member">
            <table>
                <thead>
                    <tr>
                        <th class="sort" @click="SortBy('Name')">{{ $t('member.page.name') }}</th>
                        <th class="sort" @click="SortBy('Acc')">{{ $t('member.page.acc') }}</th>
                        <th>{{ $t('common.operate') }}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in sortedItems" :key="item.Id">
                        <td>{{ item.Name }}</td>
                        <td>{{ item.Acc }}</td>
                        <td>
                            <input v-if="updatePermission" type="button" class="btn update" :value="$t('common.update')" @click="OpenUpdate(item.Id)"/>
                        </td>
                    </tr>
                </tbody>
            </table>

            <div class="popup" v-if="showPopup">
                <div class="popup_head">
                    <h5>{{ $t('common.update') }}</h5>
                </div>

                <div class="popup_data">
                    <div>
                        <label>{{ $t('member.page.acc') }}</label><br><br>
                        <input type="text" class="text" maxlength="16" v-model="acc" disabled><br><br>

                        <label><label class="required_mark">*</label>{{ $t('member.page.level') }}</label><br/>
                        <select class="select" v-model="level">
                            <option value="1">LV1</option>
                            <option value="2">LV2</option>
                            <option value="3">LV3</option>
                            <option value="4">LV4</option>
                            <option value="5">LV5</option>
                        </select>
                        <br/><br/>

                        <label><label class="required_mark">*</label>{{ $t('member.page.enabled') }}</label><br/>
                        <select class="select" v-model="enabled">
                            <option value="1">{{ $t('common.vaild') }}</option>
                            <option value="0">{{ $t('common.inVaild') }}</option>
                        </select>
                        <br/><br/>
                    </div>

                    <div align="right">
                        <input type="button" class="btn submit" :value="$t('common.submit')" @click="UpdateMember()"/>
                        <input type="button" class="btn cancel" :value="$t('common.cancel')" @click="ClosePopup()"/>
                    </div>
                </div>
            </div>

            <div class="overlay" v-if="showPopup"></div>
        </div>
    `,
    data() {
        return {
            memberData: {},
            showPopup: false,
            memberId: 0,
            memberName: '',
            acc: '',
            level: 1,
            enabled: 1,
            sortKey: '',
            sortDesc: false,
            originMemberData: {},
            updatePermission: true
        }
    },
    created: function () {
        this.GetMemberData();
    },
    mounted() {
        window.addEventListener('keydown', this.HandleKeyDown);
    },
    methods: {
        async GetMemberData() {
            await fetch('/api/member/getMemberData', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    this.memberData = myJson
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
        async UpdateMember() {

            const data = {
                MemberId: this.memberId,
                Level: this.level,
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
                await fetch('/api/member/updateMemberData', {
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
        OpenUpdate(id) {
            let updateData = this.memberData.find(item => item.Id === id);

            this.showPopup = true;
            this.memberId = id;
            this.memberName = updateData.Name;
            this.acc = updateData.Acc;
            this.level = updateData.Level;
            this.enabled = updateData.Enabled;

            this.originMemberData = {
                Level: updateData.Level,
                Enabled: updateData.Enabled
            };
        },
        EditDataCheck() {
            const nowData = {
                Level: this.level,
                Enabled: this.enabled
            };

            return JSON.stringify(this.originMemberData) === JSON.stringify(nowData)
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

                return this.memberData.slice().sort((a, b) => {
                    if (typeof a[key] === 'number' && typeof b[key] === 'number') {
                        return (a[key] - b[key]) * order;
                    } else {
                        return (a[key].toString().localeCompare(b[key].toString())) * order;
                    }
                });
            } else {
                return this.memberData;
            }
        }
    },
    beforeDestroy() {
        window.removeEventListener('keydown', this.HandleKeyDown);
    },
});