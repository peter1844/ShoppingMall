Vue.component('admin', {
    template: `
        <div class="admin">
            <div>
                <input type="button" class="btn insert" value="新 增"/>
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
                    <tr v-for="item in adminData">
                        <td>{{ item.Name }}</td>
                        <td>{{ item.Acc }}</td>
                        <td>
                            <input type="button" class="btn update" value="編 輯"/>
                            <input type="button" class="btn delete" value="刪 除"/>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    `,
    data() {
        return {
            adminData: {}
        }
    },
    created: function () {
        this.GetAdminData();
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
                    this.adminData = myJson;
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
    },
});