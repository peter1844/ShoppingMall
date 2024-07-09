Vue.component('login', {
    template: `
        <div class="login" align="center">
            <div>
                <img src="/images/Zgmf-X20a.jpg" class="login_logo"/><br/><br/>
                <h3>後臺管理系統</h3>
            </div>
            <div class="login_area" align="left">
                <label class="login_description">帳號</label><br/><br/>
                <input type="text" class="login_text" maxlength="16" v-model="acc" @keyup.enter="Login"/><br/><br/>

                <label class="login_description">密碼</label><br/><br/>
                <input type="password" class="login_text" maxlength="16" v-model="pwd" @keyup.enter="Login"/><br/><br/>

                <input type="button" class="login_btn" value="登 入" @click="Login">
            </div>
        </div>
    `,
    data() {
        return {
            acc: '',
            pwd: ''
        }
    },
    methods: {
        async Login() {

            if (this.acc == '' || this.pwd == '') {
                Swal.fire({
                    title: '格式異常',
                    text: '帳號或密碼不得為空',
                    icon: "error",
                    confirmButtonText: '確認'
                })

                return false;
            }

            const validCharacters = /^[a-zA-Z0-9]+$/;
            if (!validCharacters.test(this.acc) || !validCharacters.test(this.pwd)) {
                Swal.fire({
                    title: '格式異常',
                    text: '帳號或密碼不得有特殊字元',
                    icon: "error",
                    confirmButtonText: '確認'
                })

                return false;
            }

            const data = {
                Acc: this.acc,
                Pwd: this.pwd
            };

            await fetch('/api/login/checkLoginByAccPwd', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data)
            }).then((response) => {
                if (response.status !== 200) throw new Error(response.status)
                return response.json()
            }).then((myJson) => {
                localStorage.setItem('adminName', myJson[0].Name);
                localStorage.setItem('token', myJson[0].Token);

                window.location.href = '/Views/Index.aspx';
            }).catch((error) => {
                Swal.fire({
                    title: '登入失敗',
                    text: '請確認帳號狀態及帳號密碼是否有誤',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        },
    },
});