Vue.component('login', {
    template: `
        <div class="login" align="center">
            <div>
                <img src="/images/Zgmf-X20a.jpg" class="login_logo"/><br/><br/>
                <h3>{{ $t('login.page.backendManagementSystem') }}</h3>
            </div>
            <div class="login_area" align="left">
                <label class="login_description">{{ $t('login.page.acc') }}</label><br/><br/>
                <input type="text" class="login_text" maxlength="16" v-model="acc" @keyup.enter="Login"/><br/><br/>

                <label class="login_description">{{ $t('login.page.pwd') }}</label><br/><br/>
                <input type="password" class="login_text" maxlength="16" v-model="pwd" @keyup.enter="Login"/><br/><br/>

                <input type="button" class="login_btn" :value="$t('login.page.login')" @click="Login">
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
                    text: this.$t('login.message.accOrPwdEmpty'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                })

                return false;
            }

            const validCharacters = /^[a-zA-Z0-9]+$/;
            if (!validCharacters.test(this.acc) || !validCharacters.test(this.pwd)) {
                Swal.fire({
                    text: this.$t('login.message.accOrPwdSpecialChar'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    localStorage.setItem('adminName', myJson[0].Name);
                    localStorage.setItem('token', myJson[0].Token);

                    window.location.href = '/Views/Index.aspx';
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
    },
});