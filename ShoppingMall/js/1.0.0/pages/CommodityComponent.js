Vue.component('commodity', {
    template: `
        <div class="commodity">
            <div class="search_area">
                <b>{{ $t('commodity.page.name') }}</b>
                <input type="text" class="search_text" v-model="conditionName">

                <b>{{ $t('commodity.page.type') }}</b>
                <select class="search_select" v-model="conditionType">
                    <option value="">{{ $t('common.select') }}</option>
                    <option v-for="item in optionData" :key="item.CommodityId" :value="item.CommodityId">{{ item.CommodityName }}</option>
                </select>

                <input type="button" class="btn search" :value="$t('common.search')" @click="GetCommodityDataByCondition()">
            </div>
            <br/><br/>

            <div>
                <input v-if="insertPermission" type="button" class="btn insert" :value="$t('common.insert')" @click="OpenInsert()"/>
            </div>
            <br/>
            <table>
                <thead>
                    <tr>
                        <th class="sort" @click="SortBy('Name')">{{ $t('commodity.page.name') }}</th>
                        <th class="sort" @click="SortBy('CommodityName')">{{ $t('commodity.page.type') }}</th>
                        <th class="sort" @click="SortBy('Price')">{{ $t('commodity.page.price') }}</th>
                        <th class="sort" @click="SortBy('Stock')">{{ $t('commodity.page.stock') }}</th>
                        <th>{{ $t('common.operate') }}</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in sortedItems" :key="item.Id">
                        <td>{{ item.Name }}</td>
                        <td>{{ item.CommodityName }}</td>
                        <td>{{ item.Price }}</td>
                        <td>{{ item.Stock }}</td>
                        <td>
                            <input v-if="updatePermission" type="button" class="btn update" :value="$t('common.update')" @click="OpenUpdate(item.Id)"/>
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
                        <label><label class="required_mark">*</label>{{ $t('commodity.page.name') }}</label><br><br>
                        <input type="text" class="text" maxlength="50" v-model="commodityName"><br><br>

                        <label>{{ $t('commodity.page.description') }}</label><br><br>
                        <textarea class="textarea" maxlength="200" rows="4" v-model="description"></textarea><br><br>

                        <label><label class="required_mark">*</label>{{ $t('commodity.page.type') }}</label><br/>
                        <select class="select" v-model="type">
                            <option value="">{{ $t('common.select') }}</option>
                            <option v-for="item in optionData" :key="item.CommodityId" :value="item.CommodityId">{{ item.CommodityName }}</option>
                        </select><br/><br/>

                        <label>{{ $t('commodity.page.image') }}</label><br/><br/>
                        <input type="file" accept="image/jpeg, image/png, image/gif" @change="CheckFileType($event)"><br/><br/>

                        <img :src="imagePath" class="preview_img" v-if="showImage"/>
                        <span class="img_delete" v-if="showImage" @click="DeleteImage()">x</span>
                        <br/><br/>

                        <label><label class="required_mark">*</label>{{ $t('commodity.page.price') }}</label><br><br>
                        <input type="number" class="text" v-model="price"><br><br>

                        <label><label class="required_mark">*</label>{{ $t('commodity.page.stock') }}</label><br><br>
                        <input type="number" class="text" v-model="stock"><br><br>

                        <label><label class="required_mark">*</label>{{ $t('commodity.page.enabled') }}</label><br/>
                        <select class="select" v-model="open">
                            <option value="1">{{ $t('commodity.page.open') }}</option>
                            <option value="0">{{ $t('commodity.page.close') }}</option>
                        </select>
                        <br/><br/>
                    </div>
                    
                    <div align="right">
                        <input type="button" class="btn submit" :value="$t('common.submit')" @click="CheckAction()"/>
                        <input type="button" class="btn cancel" :value="$t('common.cancel')" @click="ClosePopup()"/>
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
            deleteImgFlag: false,
            originCommodityData: {},
            insertPermission: false,
            updatePermission: false
        }
    },
    created: function () {
        this.GetCommodityPermissionData();
        this.GetCommodityData();
        this.GetOptionData();
    },
    mounted() {
        window.addEventListener('keydown', this.HandleKeyDown);
    },
    methods: {
        async GetCommodityPermissionData() {
            await fetch('/api/commodity/getCommodityPermissions', {
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
                        text: this.$t('common.searchCompleted'),
                        icon: "success",
                        confirmButtonText: this.$t('common.confirm')
                    })

                    this.commodityData = myJson;
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
                console.log(error.message);
                Swal.fire({
                    text: this.$t('common.systemError'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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
        async InsertCommodity() {

            if (this.commodityName == '' || this.type == '' || this.price == '' || this.stock == '') {
                Swal.fire({
                    text: this.$t('commodity.message.requiredYet'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                });

                return false;
            }

            const validCharacters = /^[1-9][0-9]*$/;
            if (!validCharacters.test(this.price) || !validCharacters.test(this.stock)) {
                Swal.fire({
                    text: this.$t('commodity.message.priceOrStockFormatError'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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
        async UpdateCommodity() {

            if (this.commodityName == '' || this.type == '' || this.price == '' || this.stock == '') {
                Swal.fire({
                    text: this.$t('commodity.message.requiredYet'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                });

                return false;
            }

            const validCharacters = /^[1-9][0-9]*$/;
            if (!validCharacters.test(this.price) || !validCharacters.test(this.stock)) {
                Swal.fire({
                    text: this.$t('commodity.message.priceOrStockFormatError'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
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

            let noChangeFlag = this.EditDataCheck();

            if (noChangeFlag) {
                this.showPopup = false;

                Swal.fire({
                    text: this.$t('common.noChangeData'),
                    icon: "success",
                    confirmButtonText: this.$t('common.confirm')
                })
            } else {
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
            this.actionText = this.$t('common.insert');
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
            this.actionText = this.$t('common.update');
            this.deleteImgFlag = false;

            this.originCommodityData = {
                Name: updateData.Name,
                Description: updateData.Description,
                Type: updateData.Type,
                Price: updateData.Price,
                Stock: updateData.Stock,
                Open: updateData.Open
            };
        },
        EditDataCheck() {
            const nowData = {
                Name: this.commodityName,
                Description: this.description,
                Type: this.type,
                Price: this.price,
                Stock: this.stock,
                Open: this.open
            };

            if (JSON.stringify(this.originCommodityData) === JSON.stringify(nowData)) {

                if (this.uploadFile == '' && !this.deleteImgFlag) {
                    return true;
                } else {
                    return false;
                }

            } else {
                return false;
            }

            return JSON.stringify(this.originCommodityData) === JSON.stringify(nowData)
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
                        text: this.$t('commodity.message.imageFormatError'),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
                    });

                    event.target.value = '';
                } else {
                    
                    // 檔案大小不得超過300KB
                    if (event.target.files[0].size > 1024 * 300) {
                        Swal.fire({
                            text: this.$t('commodity.message.imageSizeError'),
                            icon: "error",
                            confirmButtonText: this.$t('common.confirm')
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