Vue.component('admin', {
    template: `
        <div>
            <div>
                <input type="button" value="新增帳號">
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
                    <tr>
                        <td>Peter</td>
                        <td>r1Kaq34Nuu</td>
                        <td>
                        <button>編輯</button>
                        <button>删除</button>
                        </td>
                    </tr>
                    <tr>
                        <td>Phoebe</td>
                        <td>Ah735ecUo08</td>
                        <td>
                        <button>編輯</button>
                        <button>删除</button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    `,
    data() {
        return {

        }
    },
    created: function () {

    },
    methods: {
        
    },
});