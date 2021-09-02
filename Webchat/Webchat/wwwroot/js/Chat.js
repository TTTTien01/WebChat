//ket noi
var connection = new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.start();

var pageData = {
    selectedUserId: null,
    currentUserId: $("#currentUserId").val() //# chọn theo Id
};

$(".user-item").click(function (ev) {
    //xóa thẻ đc chọn trước đó
    $(".user-item.selected").removeClass("selected");
    $(ev.currentTarget).addClass("selected");
    //lưu lại id của user đc chọn
    pageData.selectedUserId = $(ev.currentTarget).attr("data-user-id")
});


$("#input-msg").keydown(function (ev) {
    //13 là phím enter
    if (ev.keyCode == 13 && ev.shiftKey == false) {
        //lấy nội dung ; 
        var mesg = $(ev.currentTarget).val();
        //gửi tin nhắn
        connection.invoke("Guitinnhan", pageData.selectedUserId, mesg)
            .then(function () {
                //sau khi gửi tn thì xóa tn đó
                $(ev.currentTarget).val("");
            });
    }
});

//sự kiện nhận tin nhắn
connection.on("Phanhoitinnhan", function (response) {
    var template = `<div class="msg-box">
                        <div class="msg-content">${response.mesg} </div>
                        <div class="msg-time">${response.datetime}</div>
                    </div>`;
//  Nếu đây là tn của mình gửi  
    var element = $(template);
    if (pageData.currentUserId == response.sender) {
        element.addClass("me");
    }
    var container = $(".msg-box-container");
    container.append(element);
    //lăn xuống cuối
    container.scrollTop(container[0].scrollHeight);
    
})