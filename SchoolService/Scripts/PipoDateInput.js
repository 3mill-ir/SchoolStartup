$('#From').keyup(function () {
    var date = this.value;
    if (date.match(/^\d{4}\/\d{2}\/\d{3}$/) !== null) {
        this.value = date.substring(0, date.length - 1);
    }
    if (date.match(/^\d{4}$/) !== null) {
        this.value = date + '/';
    } else if (date.match(/^\d{4}\/\d{2}$/) !== null) {
        this.value = date + '/';
    }
})
$('#To').keyup(function () {
    var date = this.value;
    if (date.match(/^\d{4}\/\d{2}\/\d{3}$/) !== null) {
        this.value = date.substring(0, date.length - 1);
    }
    if (date.match(/^\d{4}$/) !== null) {
        this.value = date + '/';
    } else if (date.match(/^\d{4}\/\d{2}$/) !== null) {
        this.value = date + '/';
    }
})