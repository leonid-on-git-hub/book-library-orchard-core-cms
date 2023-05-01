Ext.define('BookLibrary.UI.Books.view.BookTracksGrid', {
    extend: 'Ext.grid.Panel',
    alias: 'widget.booktracksgrid',
    title: 'Tracks',
    collapsible: false,
    viewConfig: {
        enableTextSelection: true
    },
    columns: [{
        dataIndex: 'userName',
        text: 'User',
        flex: 1
    }, {
        dataIndex: 'userEmail',
        text: 'Email',
        flex: 1
    }, {
        dataIndex: 'actionDate',
        text: 'Date',
        flex: 1.1,
        renderer: function (value) {
            var date = new Date(value);
            const yyyy = date.getFullYear();
            let mm = date.getMonth() + 1;
            let dd = date.getDate();
            let hours = date.getHours();
            let minutes = date.getMinutes();
            let seconds = date.getSeconds();

            if (dd < 10) dd = '0' + dd;
            if (mm < 10) mm = '0' + mm;
            if (hours < 10) hours = '0' + hours;
            if (minutes < 10) minutes = '0' + minutes;
            if (seconds < 10) seconds = '0' + seconds;

            return dd + '/' + mm + '/' + yyyy + ' ' + hours + ':' + minutes + ':' + seconds;
        }
    }, {
        dataIndex: 'action',
        text: 'Action',
        flex: 1
    }],
});