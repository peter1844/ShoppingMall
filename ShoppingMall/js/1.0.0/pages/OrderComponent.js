Vue.component('order', {
    template: `
        <div class="order">
            <div class="search_area">
                <b>{{ $t('order.page.orderId') }}</b>
                <input type="text" class="search_text" v-model="conditionId">

                <b>{{ $t('order.page.startDate') }}</b>
                <input type="date" class="search_date" v-model="conditionStartDate">

                <b>{{ $t('order.page.endDate') }}</b>
                <input type="date" class="search_date" v-model="conditionEndDate">

                <b>{{ $t('order.page.deliverState') }}</b>
                <select class="search_select" v-model="conditionDeliveryState">
                    <option value="">{{ $t('common.select') }}</option>
                    <option v-for="item in optionData.DeliveryStates" :key="item.StateId" :value="item.StateId">{{ $t('order.option.' + item.StateName) }}</option>
                </select>

                <input type="button" class="btn search" :value="$t('common.search')" @click="GetOrderDataByCondition()">
            </div>
            <br/><br/>

            <div>
                <input v-if="insertPermission" type="button" class="btn insert" :value="$t('order.page.placeAnOrder')" @click="OpenInsert()"/>
            </div>
            <br/>
            <table>
                <thead>
                    <tr>
                        <th class="sort" @click="SortBy('Id')">{{ $t('order.page.orderId') }}</th>
                        <th class="sort" @click="SortBy('MemberName')">{{ $t('order.page.memberName') }}</th>
                        <th class="sort" @click="SortBy('OrderDate')">{{ $t('order.page.orderDate') }}</th>
                        <th class="sort" @click="SortBy('TotalMoney')">{{ $t('order.page.totalMoney') }}</th>
                        <th class="sort" @click="SortBy('DeliverStateName')">{{ $t('order.page.deliverState') }}</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in sortedItems" :key="item.Id">
                        <td>{{ item.Id }}</td>
                        <td>{{ item.MemberName }}</td>
                        <td>{{ FormatDate(item.OrderDate) }}</td>
                        <td>{{ item.TotalMoney }}</td>
                        <td>{{ $t('order.option.' + item.DeliverStateName) }}</td>
                        <td>
                            <input type="button" class="btn detail" :value="$t('order.page.detail')" @click="OpenDetail(item.Id)"/>
                            <input v-if="updatePermission && item.DeliverStateId != 2" type="button" class="btn update" :value="$t('common.update')" @click="OpenUpdate(item.Id)"/>
                        </td>
                    </tr>
                </tbody>
            </table>

            <div class="popupDetail" v-if="showPopupDetail">
                
                <div class="popupDetail_head">
                    <h5>{{ $t('order.page.orderDetail') }}</h5>
                </div>
                <hr>

                <div class="popupDetail_data">
                    <table>
                        <thead>
                            <tr>
                                <th>{{ $t('order.page.commodityName') }}</th>
                                <th>{{ $t('order.page.commodityImage') }}</th>
                                <th>{{ $t('order.page.price') }}</th>
                                <th>{{ $t('order.page.quantity') }}</th>
                                <th>{{ $t('order.page.subTotal') }}</th>
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
                        <input type="button" class="btn cancel" :value="$t('common.close')" @click="ClosePopup()"/>
                    </div>
                </div>
            </div>


            <div class="popupInsert" v-if="showPopupInsert">
                <div class="popupInsert_head">
                    <h5>{{ $t('order.page.placeAnOrder') }}</h5>
                </div>

                <div class="popupInsert_data">
                    <div>
                        <label><label class="required_mark">*</label>{{ $t('order.page.payType') }}</label><br/>
                        <select class="select" v-model="insertPayType">
                            <option v-for="item in optionData.PayTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('order.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>{{ $t('order.page.deliverType') }}</label><br/>
                        <select class="select" v-model="insertDeliverType">
                            <option v-for="item in optionData.DeliveryTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('order.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <input type="button" class="btn insert" :value="$t('common.insert')" @click="addCommodityRow()"><br/>

                        <div v-for="(item, index) in insertCommodityData" :key="index">
                            <label>{{ $t('order.page.commodityName') }}</label>
                            <select class="select" v-model="item.CommodityId" @change="updatePrice(index)">
                                <option v-for="commoditys in optionData.OpenCommodityDatas" :value="commoditys.CommodityId">{{ commoditys.CommodityName }}</option>
                            </select>

                            <label style="margin-left:30px;">{{ $t('order.page.price') }}:{{ item.Price }}</label>

                            <label style="margin-left:30px;">{{ $t('order.page.quantity') }}</label>
                            <input type="number" class="number" v-model="item.Quantity">
                            <label>{{ $t('order.page.subTotal') }}:{{ item.Quantity * item.Price }}</label>
                        </div>
                    </div><br/>

                    <div align="right">
                        <input type="button" class="btn submit" :value="$t('common.submit')" @click="InsertOrder()"/>
                        <input type="button" class="btn cancel" :value="$t('common.cancel')" @click="ClosePopup()"/>
                    </div>
                </div>
            </div>


            <div class="popupUpdate" v-if="showPopupUpdate">
                <div class="popupUpdate_head">
                    <h5>{{ $t('common.update') }}</h5>
                </div>

                <div class="popupUpdate_data">
                    <div>
                        <label>{{ $t('order.page.orderId') }}</label><br><br>
                        <input type="text" class="text" v-model="orderId" disabled><br><br>

                        <label><label class="required_mark">*</label>{{ $t('order.page.payType') }}</label><br/>
                        <select class="select" v-model="payType" :disabled="editPayTypeDisabled">
                            <option v-for="item in optionData.PayTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('order.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>{{ $t('order.page.payState') }}</label><br/>
                        <select class="select" v-model="payState" @change="ChangePayStateCheck">
                            <option v-for="item in optionData.PayStates" :key="item.StateId" :value="item.StateId">{{ $t('order.option.' + item.StateName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>{{ $t('order.page.deliverType') }}</label><br/>
                        <select class="select" v-model="deliverType" :disabled="editDeliverTypeDisabled">
                            <option v-for="item in optionData.DeliveryTypes" :key="item.TypeId" :value="item.TypeId">{{ $t('order.option.' + item.TypeName) }}</option>
                        </select><br/><br/>

                        <label><label class="required_mark">*</label>{{ $t('order.page.deliverState') }}</label><br/>
                        <select class="select" v-model="deliverState" @change="ChangeDeliverStateCheck">
                            <option v-for="item in optionData.DeliveryStates" :key="item.StateId" :value="item.StateId">{{ $t('order.option.' + item.StateName) }}</option>
                        </select><br/><br/>
                    </div>

                    <div align="right">
                        <input type="button" class="btn submit" :value="$t('common.submit')" @click="UpdateOrder()"/>
                        <input type="button" class="btn cancel" :value="$t('common.cancel')" @click="ClosePopup()"/>
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
                        text: this.$t('common.searchCompleted'),
                        icon: "success",
                        confirmButtonText: this.$t('common.confirm')
                    })

                    this.orderData = myJson;
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
        async InsertOrder() {

            const allCommodityId = this.insertCommodityData.map(item => item.CommodityId);
            const uniqueCommodityId = new Set(allCommodityId);

            if (allCommodityId.length != uniqueCommodityId.size) {
                Swal.fire({
                    text: this.$t('order.message.repeatCommodity'),
                    icon: "error",
                    confirmButtonText: this.$t('common.confirm')
                })

                return false;
            }

            const validCharacters = /^[1-9][0-9]*$/;
            let editFlag = true;
            this.insertCommodityData.forEach((item, index) => {

                if (!validCharacters.test(item.Quantity)) {
                    Swal.fire({
                        text: this.$t('order.message.quantityFormatError'),
                        icon: "error",
                        confirmButtonText: this.$t('common.confirm')
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
            }
        },
        async UpdateOrder() {
            var actionFlag = true;

            if (this.deliverState == 2) {
                await Swal.fire({
                    html: this.$t('order.message.returnAlert') + '<label style="color:red;">' + this.$t('order.option.Return') + '</label>' + this.$t('order.message.returnAlert2'),
                    icon: "question",
                    confirmButtonText: this.$t('common.confirm'),
                    showCancelButton: true,
                    cancelButtonText: this.$t('common.cancelNoSpace'),
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
                        text: this.$t('common.noChangeData'),
                        icon: "success",
                        confirmButtonText: this.$t('common.confirm')
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
            }
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