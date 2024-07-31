Vue.component('order', {
    template: `
        <div class="order">
            <div class="search_area">
                <b>訂單編號</b>
                <input type="text" class="search_text" v-model="conditionId">

                <b>訂單日期-起</b>
                <input type="date" class="search_date" v-model="conditionStartDate">

                <b>訂單日期-迄</b>
                <input type="date" class="search_date" v-model="conditionEndDate">

                <b>配送狀態</b>
                <select class="search_select" v-model="conditionDeliveryState">
                    <option value="">請選擇</option>
                    <option v-for="item in optionData.DeliveryStates" :key="item.StateId" :value="item.StateId">{{ $t('orderPage.option.' + item.StateName) }}</option>
                </select>

                <input type="button" class="btn search" value="查 詢" @click="GetOrderDataByCondition()">
            </div>
            <br/><br/>

            <div>
                <input v-if="insertPermission" type="button" class="btn insert" value="模擬下單" @click="OpenInsert()"/>
                <input v-if="deletePermission" type="button" class="btn delete" value="刪除訂單" @click="DeleteOrder()"/>
            </div>
            <br/>
            <table>
                <thead>
                    <tr>
                        <th class="sort" @click="SortBy('Id')">訂單編號</th>
                        <th class="sort" @click="SortBy('MemberName')">會員名字</th>
                        <th class="sort" @click="SortBy('OrderDate')">訂單日期</th>
                        <th class="sort" @click="SortBy('TotalMoney')">總金額</th>
                        <th class="sort" @click="SortBy('DeliverStateName')">配送狀態</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in sortedItems" :key="item.Id">
                        <td>{{ item.Id }}</td>
                        <td>{{ item.MemberName }}</td>
                        <td>{{ FormatDate(item.OrderDate) }}</td>
                        <td>{{ item.TotalMoney }}</td>
                        <td>{{ $t('orderPage.option.' + item.DeliverStateName) }}</td>
                        <td>
                            <input type="button" class="btn detail" value="明 細" @click="OpenDetail(item.Id)"/>
                            <input v-if="updatePermission && item.DeliverStateId != 2" type="button" class="btn update" value="編 輯" @click="OpenUpdate(item.Id)"/>
                        </td>
                    </tr>
                </tbody>
            </table>

            <div class="popupDetail" v-if="showPopupDetail">
                
                <div class="popupDetail_head">
                    <h5>訂單明細</h5>
                </div>
                <hr>

                <div class="popupDetail_data">
                    <table>
                        <thead>
                            <tr>
                                <th>商品名稱</th>
                                <th>商品圖片</th>
                                <th>單價</th>
                                <th>數量</th>
                                <th>小計</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="item in orderDetailData">
                                <td>{{ item.CommodityName }}</td>
                                <td><img :src="item.Image" @error="handleImageError" class="commodityImage"></td>
                                <td>{{ item.Price }}</td>
                                <td>{{ item.Quantity }}</td>
                                <td>{{ item.Price * item.Quantity }}</td>
                            </tr>
                        </tbody>
                    </table>

                    <div align="right">
                        <input type="button" class="btn cancel" value="關 閉" @click="ClosePopup()"/>
                    </div>
                </div>
            </div>


            <div class="popupInsert" v-if="showPopupInsert">
                <div class="popupInsert_head">
                    <h5>模擬下單</h5>
                </div>

                <div class="popupInsert_data">
                    <div>
                        <label><label class="required_mark">*</label>付款方式</label><br/>
                        <select class="select" v-model="insertPayType">
                            <option v-for="item in optionData.PayTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('orderPage.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>配送方式</label><br/>
                        <select class="select" v-model="insertDeliverType">
                            <option v-for="item in optionData.DeliveryTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('orderPage.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <input type="button" class="btn insert" value="新增" @click="addCommodityRow()"><br/>

                        <div v-for="(item, index) in insertCommodityData" :key="index">
                            <label>商品名稱</label>
                            <select class="select" v-model="item.CommodityId" @change="updatePrice(index)">
                                <option v-for="commoditys in optionData.OpenCommodityDatas" :value="commoditys.CommodityId">{{ commoditys.CommodityName }}</option>
                            </select>

                            <label style="margin-left:30px;">單價:{{ item.Price }}</label>

                            <label style="margin-left:30px;">數量</label>
                            <input type="number" class="number" v-model="item.Quantity">
                            <label>小計:{{ item.Quantity * item.Price }}</label>
                        </div>
                    </div><br/>

                    <div align="right">
                        <input type="button" class="btn submit" value="送 出" @click="InsertOrder()"/>
                        <input type="button" class="btn cancel" value="取 消" @click="ClosePopup()"/>
                    </div>
                </div>
            </div>


            <div class="popupUpdate" v-if="showPopupUpdate">
                <div class="popupUpdate_head">
                    <h5>編 輯</h5>
                </div>

                <div class="popupUpdate_data">
                    <div>
                        <label>訂單編號</label><br><br>
                        <input type="text" class="text" v-model="orderId" disabled><br><br>

                        <label><label class="required_mark">*</label>付款方式</label><br/>
                        <select class="select" v-model="payType" :disabled="editPayTypeDisabled">
                            <option v-for="item in optionData.PayTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('orderPage.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>付款狀態</label><br/>
                        <select class="select" v-model="payState" @change="ChangePayStateCheck">
                            <option v-for="item in optionData.PayStates" :key="item.StateId" :value="item.StateId">{{ $t('orderPage.option.' + item.StateName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>配送方式</label><br/>
                        <select class="select" v-model="deliverType" :disabled="editDeliverTypeDisabled">
                            <option v-for="item in optionData.DeliveryTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('orderPage.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>配送狀態</label><br/>
                        <select class="select" v-model="deliverState" @change="ChangeDeliverStateCheck">
                            <option v-for="item in optionData.DeliveryStates" :key="item.StateId" :value="item.StateId">{{ $t('orderPage.option.' + item.StateName) }}</option>
                        </select><br/><br/>
                    </div>

                    <div align="right">
                        <input type="button" class="btn submit" value="送 出" @click="UpdateOrder()"/>
                        <input type="button" class="btn cancel" value="取 消" @click="ClosePopup()"/>
                    </div>
                </div>
            </div>

            <div class="overlay" v-if="showOverlay"></div>
        </div>
    `,
    data() {
        return {
            orderData: {},
            orderDetailData: {},
            optionData: {},
            insertCommodityData: [],
            showPopupDetail: false,
            showPopupInsert: false,
            showPopupUpdate: false,
            showOverlay: false,
            orderId: '',
            payType: '',
            payState: '',
            deliverType: '',
            deliverState: '',
            sortKey: '',
            sortDesc: false,
            conditionId: '',
            conditionStartDate: '',
            conditionEndDate: '',
            conditionDeliveryState: '',
            insertPayType: '',
            insertDeliverType: '',
            defaultImage: '/images/commodity/default.jpg',
            editPayTypeDisabled: false,
            editDeliverTypeDisabled: false,
            originPayType: '',
            originDeliverType: '',
            originOrderData: {},
            insertPermission: false,
            updatePermission: false,
            deletePermission: false
        }
    },
    created: function () {
        this.GetOrderPermissionData();
        this.GetOrderData();
        this.GetOptionData();
    },
    mounted() {
        window.addEventListener('keydown', this.HandleKeyDown);
    },
    methods: {
        async GetOrderPermissionData() {
            await fetch('/api/order/getOrderPermissions', {
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
        async GetOrderData() {

            const params = new URLSearchParams();

            params.append('Id', this.conditionId);
            params.append('StartDate', this.conditionStartDate);
            params.append('EndDate', this.conditionEndDate);
            params.append('DeliveryState', this.conditionDeliveryState);

            await fetch('/api/order/getOrderData', {
                headers: {
                    'token': localStorage.getItem('token')
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    this.orderData = myJson;
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
        async GetOrderDataByCondition() {

            const params = new URLSearchParams();

            params.append('Id', this.conditionId);
            params.append('StartDate', this.conditionStartDate);
            params.append('EndDate', this.conditionEndDate);
            params.append('DeliveryState', this.conditionDeliveryState);

            await fetch(`/api/order/getOrderData?${params.toString()}`, {
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

                    this.orderData = myJson;
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
            await fetch('/api/order/getOrderOptionData', {
                headers: {
                    'token': localStorage.getItem('token'),
                    'Content-Type': 'application/json',
                },
            }).then((response) => {
                return response.json()
            }).then((myJson) => {
                if (myJson.ErrorMessage === undefined) {
                    this.optionData = myJson[0];
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
        async InsertOrder() {

            const allCommodityId = this.insertCommodityData.map(item => item.CommodityId);
            const uniqueCommodityId = new Set(allCommodityId);

            if (allCommodityId.length != uniqueCommodityId.size) {
                Swal.fire({
                    text: '重覆商品請合併填寫',
                    icon: "error",
                    confirmButtonText: '確認'
                })

                return false;
            }

            const validCharacters = /^[1-9][0-9]*$/;
            let editFlag = true;
            this.insertCommodityData.forEach((item, index) => {

                if (!validCharacters.test(item.Quantity)) {
                    Swal.fire({
                        text: '購買數量格式錯誤',
                        icon: "error",
                        confirmButtonText: '確認'
                    })

                    editFlag = false;
                    return;
                }

            });

            if (editFlag) {
                const data = {
                    MemberId: 2,
                    PayType: this.insertPayType,
                    DeliverType: this.insertDeliverType,
                    TotalMoney: this.totalAmount,
                    CommodityDatas: this.insertCommodityData,
                };

                fetch('/api/order/insertOrderData', {
                    method: 'POST',
                    headers: {
                        'token': localStorage.getItem('token'),
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(data)
                }).then((response) => {
                    this.showPopupInsert = false;
                    this.showOverlay = false;

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
            }
        },
        async UpdateOrder() {
            var actionFlag = true;

            if (this.deliverState == 2) {
                await Swal.fire({
                    html: '將配送狀態修改為 <label style="color:red;">' + this.$t('orderPage.option.Return') + '</label> 後將不能編輯此訂單，是否執行？',
                    icon: "question",
                    confirmButtonText: '確認',
                    showCancelButton: true,
                    cancelButtonText: '取消',
                }).then((result) => {
                    if (!result.isConfirmed) {
                        actionFlag = false
                    }
                });
            }

            if (actionFlag) {
                const data = {
                    OrderId: this.orderId,
                    PayTypeId: this.payType,
                    PayStateId: this.payState,
                    DeliverTypeId: this.deliverType,
                    DeliverStateId: this.deliverState
                };

                let noChangeFlag = this.EditDataCheck();

                if (noChangeFlag) {
                    this.showPopupUpdate = false;
                    this.showOverlay = false;

                    Swal.fire({
                        text: '無異動資料',
                        icon: "success",
                        confirmButtonText: '確認'
                    })
                } else {
                    fetch('/api/order/updateOrderData', {
                        method: 'PUT',
                        headers: {
                            'token': localStorage.getItem('token'),
                            'Content-Type': 'application/json',
                        },
                        body: JSON.stringify(data)
                    }).then((response) => {
                        this.showPopupUpdate = false;
                        this.showOverlay = false;

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
                }
            }
        },
        async DeleteOrder() {

            await Swal.fire({
                html: '確定要刪除 <label style="color:red;">180</label> 天前的訂單嗎？',
                icon: "question",
                confirmButtonText: '確認',
                showCancelButton: true,
                cancelButtonText: '取消',
            }).then((result) => {
                if (result.isConfirmed) {

                    fetch('/api/order/deleteOrderData', {
                        method: 'DELETE',
                        headers: {
                            'token': localStorage.getItem('token'),
                            'Content-Type': 'application/json',
                        }
                    }).then((response) => {
                        return response.json()
                    }).then((myJson) => {
                        if (myJson.ErrorMessage === undefined) {
                            Swal.fire({
                                text: '刪除完成',
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
                }
            });
        },
        OpenDetail(id) {
            let detailData = this.orderData.find(item => item.Id === id);

            this.orderDetailData = detailData.DetailDatas;
            this.showPopupDetail = true;
            this.showOverlay = true;
        },
        OpenInsert() {
            this.showPopupInsert = true;
            this.showOverlay = true;
            this.insertPayType = this.optionData.PayTypes[0].TypeId;
            this.insertDeliverType = this.optionData.DeliveryTypes[0].TypeId;
            this.insertCommodityData = [];
            this.insertCommodityData.push({
                CommodityId: this.optionData.OpenCommodityDatas[0].CommodityId,
                Price: this.optionData.OpenCommodityDatas[0].CommodityPrice,
                Quantity: 1
            });
        },
        OpenUpdate(id) {
            let updateData = this.orderData.find(item => item.Id === id);

            this.showPopupUpdate = true;
            this.showOverlay = true;
            this.orderId = id;
            this.payType = updateData.PayTypeId;
            this.payState = updateData.PayStateId;
            this.deliverType = updateData.DeliverTypeId;
            this.deliverState = updateData.DeliverStateId;
            this.editPayTypeDisabled = this.payState == 1 ? true : false;
            this.editDeliverTypeDisabled = this.deliverState == 1 ? true : false;
            this.originPayType = updateData.PayStateId == 1 ? updateData.PayTypeId : '';
            this.originDeliverType = updateData.DeliverStateId == 1 ? updateData.DeliverTypeId : '';

            this.originOrderData = {
                PayTypeId: updateData.PayTypeId,
                PayStateId: updateData.PayStateId,
                DeliverTypeId: updateData.DeliverTypeId,
                DeliverStateId: updateData.DeliverStateId
            };
        },
        EditDataCheck() {
            const nowData = {
                PayTypeId: this.payType,
                PayStateId: this.payState,
                DeliverTypeId: this.deliverType,
                DeliverStateId: this.deliverState
            };

            return JSON.stringify(this.originOrderData) === JSON.stringify(nowData)
        },
        ClosePopup() {
            this.showPopupInsert = false;
            this.showPopupUpdate = false;
            this.showPopupDetail = false;
            this.showOverlay = false;
        },
        ChangePayStateCheck() {
            this.editPayTypeDisabled = this.payState == 0 ? false : true;
            if (this.originPayType != '' && this.payState != 0) this.payType = this.originPayType;
        },
        ChangeDeliverStateCheck() {
            this.editDeliverTypeDisabled = this.deliverState == 0 ? false : true;
            if (this.originDeliverType != '' && this.deliverState != 0) this.deliverType = this.originDeliverType;
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
        },
        FormatDate(dateData) {
            return dateData.split('T')[0];
        },
        addCommodityRow() {
            this.insertCommodityData.push({
                CommodityId: this.optionData.OpenCommodityDatas[0].CommodityId,
                Price: this.optionData.OpenCommodityDatas[0].CommodityPrice,
                Quantity: 1
            });
        },
        updatePrice(index) {
            let commodityPrice = this.optionData.OpenCommodityDatas.find(item => item.CommodityId === this.insertCommodityData[index].CommodityId).CommodityPrice;

            this.$set(this.insertCommodityData, index, {
                ...this.insertCommodityData[index],
                Price: commodityPrice
            });
        },
        handleImageError(event) {
            event.target.src = this.defaultImage;
        }
    },
    computed: {
        sortedItems() {
            if (this.sortKey) {
                let key = this.sortKey;
                let order = this.sortDesc ? -1 : 1;

                return this.orderData.slice().sort((a, b) => {
                    if (typeof a[key] === 'number' && typeof b[key] === 'number') {
                        return (a[key] - b[key]) * order;
                    } else {
                        return (a[key].toString().localeCompare(b[key].toString())) * order;
                    }
                });
            } else {
                return this.orderData;
            }
        },
        totalAmount() {
            return this.insertCommodityData.reduce((total, item) => {
                return total + (item.Price * item.Quantity);
            }, 0);
        }
    },
    beforeDestroy() {
        window.removeEventListener('keydown', this.HandleKeyDown);
    },
});