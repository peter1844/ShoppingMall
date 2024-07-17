Vue.component('member', {
    template: `
        <div class="member">
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
                        </td>
                    </tr>
                </tbody>
            </table>

            <div class="popup" v-if="showPopup">
                <div class="popup_head">
                    <h5>編 輯</h5>
                </div>

                <div class="popup_data">
                    <div>
                        <label>帳號</label><br><br>
                        <input type="text" class="text" maxlength="16" v-model="acc" readonly><br><br>

                        <label><label class="required_mark">*</label>會員等級</label><br/>
                        <select class="select" v-model="level">
                            <option value="1">LV1</option>
                            <option value="2">LV2</option>
                            <option value="3">LV3</option>
                            <option value="4">LV4</option>
                            <option value="5">LV5</option>
                        </select>
                        <br/><br/>

                        <label><label class="required_mark">*</label>啟用狀態</label><br/>
                        <select class="select" v-model="enabled">
                            <option value="1">有效</option>
                            <option value="0">無效</option>
                        </select>
                        <br/><br/>
                    </div>

                    <div align="right">
                        <input type="button" class="btn submit" value="送 出" @click="UpdateMember()"/>
                        <input type="button" class="btn cancel" value="取 消" @click="ClosePopup()"/>
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
            sortDesc: false
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
                if (myJson.StatusErrorCode === undefined) {
                    this.memberData = myJson
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
        async UpdateMember() {

            const data = {
                MemberId: this.memberId,
                Level: this.level,
                Enabled: this.enabled
            };

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
        OpenUpdate(id) {
            let updateData = this.memberData.find(item => item.Id === id);

            this.showPopup = true;
            this.memberId = id;
            this.memberName = updateData.Name;
            this.acc = updateData.Acc;
            this.level = updateData.Level;
            this.enabled = updateData.Enabled;
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