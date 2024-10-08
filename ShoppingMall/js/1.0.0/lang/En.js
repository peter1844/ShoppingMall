﻿const en = {
    common: {
        confirm: 'confirm',
        go: 'go',
        operate: 'operate',
        insert: 'insert',
        update: 'edit',
        delete: 'delete',
        search: 'search',
        insertCompleted: 'Insert completed',
        updateCompleted: 'Update completed',
        deleteCompleted: 'Delete completed',
        searchCompleted: 'Search completed',
        submit: 'submit',
        cancel: 'cancel',
        cancelNoSpace: 'cancel',
        close: 'close',
        select: 'Please selected',
        vaild: 'vaild',
        inVaild: 'invaild',
        noChangeData: 'No change data',
        systemError: 'System exception,please try again later',
        backendMessage: {
            InvaildInputData: 'Data format error',
            Success: 'Success',
            DbError: 'Database error',
            InvaildToken: 'Login status invalid',
            NoHeaderToken: 'Token data anomaly',
            InvaildLogin: 'Login fail',
            NoPermission: 'Insufficient permissions',
            StockError: 'Inventory shortage',
            ExceptionError: 'Ststem error',
        },
    },
    login: {
        page: {
            backendManagementSystem: 'Back Management System',
            acc: 'account',
            pwd: 'password',
            login: 'login',
        },
        message: {
            accOrPwdEmpty: 'Account or password cannot be empty',
            accOrPwdSpecialChar: 'Account or password must not contain special characters',
        },
    },
    menu: {
        page: {
            member: 'Member management',
            commodity: 'Commodity management',
            order: 'Order management',
            admin: 'Admin management',
            logout: 'Logout',
            welcomeText: 'Hi, ',
            tw: 'Chinese',
            en: 'English',
        },
        message: {
            goAlert: 'It has been detected that ',
            goAlert2: ' commodity is out of stock. Do you want to go to the commodity page?',
        },
    },
    index: {
        page: {
            index: 'Front page',
            slogan: 'Yo! this is front page you know',
        },
    },
    member: {
        page: {
            member: 'Member management',
            name: 'name',
            acc: 'account',
            level: 'level',
            enabled: 'Enabled status',
        },
    },
    commodity: {
        page: {
            commodity: 'Commodity management',
            name: 'Commodity name',
            description: 'Commodity description',
            type: 'Commodity type',
            image: 'Commodity icon',
            price: 'price',
            stock: 'stock',
            enabled: 'Open status',
            open: 'open',
            close: 'close',
        },
        message: {
            requiredYet: 'There are still required fields left unfilled',
            priceOrStockFormatError: 'Price or stock format error',
            imageFormatError: 'Upload types only allow JPEG, PNG, GIF',
            imageSizeError: 'Upload file size exceeds 300KB',
        },
    },
    order: {
        page: {
            order: 'Order management',
            orderId: 'Order ID',
            orderDate: 'Order date',
            startDate: 'Order start date',
            endDate: 'Order end date',
            totalMoney: 'Lump sum',
            payType: 'Payment method',
            payState: 'Payment status',
            deliverType: 'Delivery method',
            deliverState: 'Delivery status',
            placeAnOrder: 'insert',
            deleteAnOrder: 'delete',
            memberName: 'Member name',
            detail: 'details',
            orderDetail: 'Order details',
            commodityName: 'Commodity name',
            commodityImage: 'Commodity icon',
            price: 'price',
            quantity: 'quantity',
            subTotal: 'Sub total',
        },
        message: {
            repeatCommodity: 'Please fill in the duplicate items together',
            quantityFormatError: 'Purchase quantity format error',
            returnAlert: 'This order cannot be edited after changing the delivery status to ',
            returnAlert2: '.Do you want to execute it?',
            deleteAlert: 'Are you sure you want to delete orders from ',
            deleteAlert2: ' days ago?',
        },
        option: {
            Credit: 'Swipe card online',
            Remittance: 'Remit money',
            LinePay: 'LinePay',
            CashOnDelivery: 'Cash on delivery',
            UnPaid: 'Unpaid',
            AlreadyPaid: 'Already paid',
            LandTransportation: 'Land transportation',
            Shipping: 'Shipping',
            AirTransportation: 'Air transportation',
            NotShipped: 'Not shipped',
            Shipped: 'Shipped',
            Return: 'Return',
        },
    },
    admin: {
        page: {
            admin: 'Admin management',
            name: 'name',
            acc: 'account',
            pwd: 'password',
            role: 'role',
            enabled: 'Enabled status',
            login: 'login',
        },
        message: {
            requiredYet: 'There are still required fields left unfilled',
            accOrPwdSpecialChar: 'Account or password must not contain special characters',
            deleteAlert: 'Are you sure you want to delete account ',
            deleteAlert2: '?',
            passwordNoChange: 'The password will not be changed if it is not entered',
        },
        option: {
            Admin: 'Admin',
            CustomerService: 'Customer service',
            Commodity: 'Commodity',
        },
    },
}