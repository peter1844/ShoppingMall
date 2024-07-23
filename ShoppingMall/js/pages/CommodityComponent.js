Vue.component('commodity', {
    template: `
        <div class="commodity">
            <div class="search_area">
                <b>商品名稱</b>
                <input type="text" class="search_text" v-model="conditionName">

                <b>商品類型</b>
                <select class="search_select" v-model="conditionType">
                    <option value="">請選擇</option>
                    <option v-for="item in optionData" :key="item.CommodityId" :value="item.CommodityId">{{ item.CommodityName }}</option>
                </select>

                <input type="button" class="btn search" value="查 詢" @click="GetCommodityDataByCondition()">
            </div>
            <br/><br/>

            <div>
                <input type="button" class="btn insert" value="新 增" @click="OpenInsert()"/>
            </div>
            <br/>
            <table>
                <thead>
                    <tr>
                        <th class="sort" @click="SortBy('Name')">商品名稱</th>
                        <th class="sort" @click="SortBy('CommodityName')">商品類型</th>
                        <th class="sort" @click="SortBy('Price')">價格</th>
                        <th class="sort" @click="SortBy('Stock')">庫存量</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in sortedItems" :key="item.Id">
                        <td>{{ item.Name }}</td>
                        <td>{{ item.CommodityName }}</td>
                        <td>{{ item.Price }}</td>
                        <td>{{ item.Stock }}</td>
                        <td>
                            <input type="button" class="btn update" value="編 輯" @click="OpenUpdate(item.Id)"/>
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
                        <label><label class="required_mark">*</label>商品名稱</label><br><br>
                        <input type="text" class="text" maxlength="50" v-model="commodityName"><br><br>

                        <label>商品描述</label><br><br>
                        <textarea class="textarea" maxlength="200" rows="4" v-model="description"></textarea><br><br>

                        <label><label class="required_mark">*</label>商品類型</label><br/>
                        <select class="select" v-model="type">
                            <option value="">請選擇</option>
                            <option v-for="item in optionData" :key="item.CommodityId" :value="item.CommodityId">{{ item.CommodityName }}</option>
                        </select><br/><br/>

                        <label>商品圖示</label><br/><br/>
                        <input type="file" accept="image/jpeg, image/png, image/gif" @change="CheckFileType($event)"><br/><br/>

                        <img :src="imagePath" class="preview_img" v-if="showImage"/>
                        <span class="img_delete" v-if="showImage" @click="DeleteImage()">x</span>
                        <br/><br/>

                        <label><label class="required_mark">*</label>價格</label><br><br>
                        <input type="number" class="text" v-model="price"><br><br>

                        <label><label class="required_mark">*</label>庫存量</label><br><br>
                        <input type="number" class="text" v-model="stock"><br><br>

                        <label><label class="required_mark">*</label>開啟狀態</label><br/>
                        <select class="select" v-model="open">
                            <option value="1">開啟</option>
                            <option value="0">關閉</option>
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
            commodityData: {},
            optionData: {},
            showPopup: false,
            commodityId: 0,
            commodityName: '',
            description: '',
            type: '',
            uploadFile: '',
            imagePath: '',
            showImage: false,
            price: '',
            stock: '',
            open: 1,
            actionType: '',
            sortKey: '',
            sortDesc: false,
            actionText: '',
            conditionName: '',
            conditionType: '',
            deleteImgFlag: false
        }
    },
    created: function () {
        this.GetCommodityData();
        this.GetOptionData();
    },
    mounted() {
        window.addEventListener('keydown', this.HandleKeyDown);
    },
    methods: {
        async GetCommodityData() {
            await fetch('/api/commodity/getCommodityData', {
                headers: {
                    'token': localStorage.getItem('token')
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    this.commodityData = myJson;
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: myJson.ErrorMessage,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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
        async GetCommodityDataByCondition() {

            const params = new URLSearchParams();
            params.append('Name', this.conditionName);
            params.append('Type', this.conditionType);

            await fetch(`/api/commodity/getCommodityData?${params.toString()}`, {
                headers: {
                    'token': localStorage.getItem('token'),
                }
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    Swal.fire({
                        text: '查詢成功',
                        icon: "success",
                        confirmButtonText: '確認'
                    })

                    this.commodityData = myJson;
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: myJson.ErrorMessage,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
                        icon: "error",
                        confirmButtonText: '確認'
                    })
                }
            }).catch((error) => {
                console.log(error.message);
                Swal.fire({
                    text: '系統異常，請稍後再試',
                    icon: "error",
                    confirmButtonText: '確認'
                })
            })
        },
        async GetOptionData() {
            await fetch('/api/commodity/getCommodityOptionData', {
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
                        text: myJson.ErrorMessage,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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
        async InsertCommodity() {

            if (this.commodityName == '' || this.type == '' || this.price == '' || this.stock == '') {
                Swal.fire({
                    text: '尚有必填欄位未填',
                    icon: "error",
                    confirmButtonText: '確認'
                });

                return false;
            }

            const validCharacters = /^[1-9][0-9]*$/;
            if (!validCharacters.test(this.price) || !validCharacters.test(this.stock)) {
                Swal.fire({
                    text: '價格或庫存量格式錯誤',
                    icon: "error",
                    confirmButtonText: '確認'
                })

                return false;
            }

            const formData = new FormData();

            formData.append('Name', this.commodityName);
            formData.append('Description', this.description);
            formData.append('Type', this.type);
            formData.append('Price', this.price);
            formData.append('Stock', this.stock);
            formData.append('Open', this.open);
            formData.append('ImageFile', this.uploadFile);

            await fetch('/api/commodity/insertCommodityData', {
                method: 'POST',
                headers: {
                    'token': localStorage.getItem('token'),
                },
                body: formData
            }).then((response) => {
                this.showPopup = false;

                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    Swal.fire({
                        text: '新增完成',
                        icon: "success",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        location.reload();
                    });
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: myJson.ErrorMessage,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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
        async UpdateCommodity() {

            if (this.commodityName == '' || this.type == '' || this.price == '' || this.stock == '') {
                Swal.fire({
                    text: '尚有必填欄位未填',
                    icon: "error",
                    confirmButtonText: '確認'
                });

                return false;
            }

            const validCharacters = /^[1-9][0-9]*$/;
            if (!validCharacters.test(this.price) || !validCharacters.test(this.stock)) {
                Swal.fire({
                    text: '價格或庫存量格式錯誤',
                    icon: "error",
                    confirmButtonText: '確認'
                })

                return false;
            }

            const formData = new FormData();

            formData.append('CommodityId', this.commodityId);
            formData.append('Name', this.commodityName);
            formData.append('Description', this.description);
            formData.append('Type', this.type);
            formData.append('Price', this.price);
            formData.append('Stock', this.stock);
            formData.append('Open', this.open);
            formData.append('ImageFile', this.uploadFile);
            formData.append('OldImage', this.imagePath);

            if (this.deleteImgFlag) {
                formData.append('DeleteFlag', "1");
            }

            await fetch('/api/commodity/updateCommodityData', {
                method: 'PUT',
                headers: {
                    'token': localStorage.getItem('token'),
                },
                body: formData
            }).then((response) => {
                this.showPopup = false;

                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    Swal.fire({
                        text: '編輯完成',
                        icon: "success",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        location.reload();
                    });
                } else if (myJson.ErrorMessage == 'InvaildToken') {
                    Swal.fire({
                        text: myJson.ErrorMessage,
                        icon: "error",
                        confirmButtonText: '確認'
                    }).then((result) => {
                        window.location.href = '/Views/Login.aspx';
                    });
                } else {
                    Swal.fire({
                        text: myJson.ErrorMessage,
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
        OpenInsert() {
            this.showPopup = true;
            this.commodityId = 0;
            this.commodityName = '';
            this.description = '';
            this.type = '';
            this.uploadFile = '';
            this.imagePath = '';
            this.showImage = false;
            this.price = '';
            this.stock = '';
            this.open = 1;
            this.actionType = 'insert';
            this.actionText = '新 增';
            this.deleteImgFlag = false;
        },
        OpenUpdate(id) {
            let updateData = this.commodityData.find(item => item.Id === id);

            this.showPopup = true;
            this.commodityId = id;
            this.commodityName = updateData.Name;
            this.description = updateData.Description;
            this.type = updateData.Type;
            this.uploadFile = '';
            this.imagePath = updateData.Image;
            this.showImage = this.imagePath == '' ? false : true;
            this.price = updateData.Price;
            this.stock = updateData.Stock;
            this.open = updateData.Open;
            this.actionType = 'update';
            this.actionText = '編 輯';
            this.deleteImgFlag = false;
        },
        CheckAction() {
            this.actionType == 'insert' ? this.InsertCommodity() : this.UpdateCommodity();
        },
        CheckFileType(event) {
            if (event.target.files.length > 0) {
                const file = event.target.files[0];
                const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];

                if (!allowedTypes.includes(file.type)) {
                    Swal.fire({
                        text: '上傳類型只允許JPEG, PNG, GIF',
                        icon: "error",
                        confirmButtonText: '確認'
                    });

                    event.target.value = '';
                } else {
                    
                    // 檔案大小不得超過300KB
                    if (event.target.files[0].size > 1024 * 300) {
                        Swal.fire({
                            text: '上傳檔案大小超過300KB',
                            icon: "error",
                            confirmButtonText: '確認'
                        });

                        event.target.value = '';
                    } else {
                        this.uploadFile = event.target.files[0];
                    }
                }
            } else {
                this.uploadFile = '';
            }

        },
        DeleteImage() {
            this.deleteImgFlag = true;
            this.showImage = false;
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

                return this.commodityData.slice().sort((a, b) => {
                    if (typeof a[key] === 'number' && typeof b[key] === 'number') {
                        return (a[key] - b[key]) * order;
                    } else {
                        return (a[key].toString().localeCompare(b[key].toString())) * order;
                    }
                });
            } else {
                return this.commodityData;
            }
        }
    },
    beforeDestroy() {
        window.removeEventListener('keydown', this.HandleKeyDown);
    },
});