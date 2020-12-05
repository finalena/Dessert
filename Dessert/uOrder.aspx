<%@ Page Title="" Language="C#" MasterPageFile="~/Site_user.Master" AutoEventWireup="true" EnableEventValidation = "false"  CodeBehind="uOrder.aspx.cs" Inherits="Dessert.uOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link rel="stylesheet" href="css/uOrder.css" >
<%-- Icon --%>
<link href="css/fontello.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    $(document).ready(function(){
        // initialize tooltips
        $('[data-toggle="tooltip"]').tooltip()
   
        if($(".order-remark").length > 0) {       // 有Order remark彈出視窗
            $('#OrderRemarkModal').modal('show');
        }
        // Cart Remark
        var itemRemark = "";
        $(".cart-order").on('click','.item-summary',function(){
            $("#CartRemarkModal").on('show.bs.modal', function(event){
                var item = $(event.relatedTarget)
                itemRemark = item.find(".item-remark");
                $(this).find('.modal-title').text(item.find(".item-name").text())     // 標題:品名
                $(this).find('.modal-body textarea').val(item.find(".item-remark").text()) // 備註寫入modal
            });
            $("#CartRemarkModal").on("shown.bs.modal",function(){
                $(this).find('.modal-body textarea').focus();
            });
        });

        $("#modalOk").on('click',function(){
            itemRemark.text($("#txtRemark").val());
            $("#CartRemarkModal").modal('hide');
        });
        // Menu Category List Sticky
        $('.menu-category-list').on('click','a', function (e) {
            e.preventDefault();
            var anchor = $(this).attr('href');    //取得個別點擊時的href，就是個別id的區塊 e.g #cat1
            anchor = anchor.substr(1, anchor.length);  
            const linkScroll = document.getElementById(anchor).offsetTop;   // 每個區塊的最上方距離 document 最上方有多遠
            $('#main').stop().animate({     // 加入stop() 讓選單滑動時不用等到動畫全跑完就可以點選其他選單
                scrollTop: linkScroll -43   // 43 -> order info 的高度，因為設定 fixed 所以扣掉，
            },700)                          // 但 zone1 要設定 padding-top 空間才不會被蓋掉
        });

        $("#main").bind('scroll', function() {
            var currentTop = $("#main").scrollTop();  //獲取容器已滾動距離 
   console.log(currentTop);
            if(currentTop >= 70 ){
                $("#categorylist").addClass('sticky');
            }else{
                $("#categorylist").removeClass('sticky');
            }
        });
    });
    
    // 使用者將產品+到購物車
    function getName(obj) {
        // 1.取得使用者將哪個產品加入購物車dish value
        var dish = document.getElementsByName(obj.id);
        // 2-1.如產品中還有多個選項，判斷哪個radio buutton被點選並指定值
        if (dish.length > 1) {
            for (var i = 0; i < dish.length; i++) {
                if (dish[i].checked) {
                    is_exist(dish[i].name, dish[i].dataset.serial, "(" + dish[i].dataset.other + ")", dish[i].value, dish[i].dataset.itemName);
                }
            }
        } // 2-2.該產品無radio button
        else {
            if (dish[0].dataset.other === "") {
                var other = "";
            } else {
                var other = "(" + dish[0].dataset.other + ")";
            }
            is_exist(dish[0].name, dish[0].dataset.serial, other, dish[0].value, dish[0].dataset.itemName);
        }
    }

    // 判斷使用者是否重複新增相同的產品選項 
    function is_exist(name, serial, other, value, productName) {
        var no = name + "ser" + serial;
        var itemqty = document.getElementById("qty" + name + "ser" + serial);
        // 1-1.如無新增相同產品，將產品加到購物車
        if (!itemqty) {
            // 1-1-1.購物車產品的html標籤
            var html = "";
            html += "<div class=\"cart-item item" + no + "\"><div class=\"input-group-btn\"><div><button type=\"button\" class=\"minus\" onclick='minus(\"" + name + "\",\"" + serial + "\")'> - </button></div><div><div class=\"cart-qty text-center\" id=\"qty" + no + "\">1</div></div><div><button type=\"button\" class=\"plus\" onclick='plus(\"" + name + "\",\"" + serial + "\")'> + </button></div></div><div class=\"item-summary \" data-toggle=\"modal\" data-target=\"#CartRemarkModal\"><div class=\"item-summary-name\"><div class=\"item-name\" id=\"name" + no + "\">" + productName + other + "</div><div class=\"item-remark\" id=\"remark" + no + "\"></div></div><div class=\"item-summary-price\" id=\"subtotal" + no + "\"data-item-price=\"" + value + "\">" + value + "</div></div></div>";
            // 1-1-2.取得購物車要新增產品的節點
            var cartOrder = document.querySelector(".cart-order");
            // 1-1-3.新增
            document.querySelector(".cart-null").style.display = "none";
            document.getElementById("btnSubmit").disabled = false;
            cartOrder.insertAdjacentHTML('afterbegin', html);

        } // 1-2.如果已新增相同商品，則數量遞增
        else {
            itemqty.innerText = parseInt(itemqty.innerText) + 1;

            // 1-2-1.計算產品小計
            var subtotal = document.getElementById("subtotal" + name + "ser" + serial)
            subtotal.innerHTML = parseInt(subtotal.dataset.itemPrice) * parseInt(itemqty.innerText);
        }
        setTotal();
    }

    // 購物車按-號
    function minus(name, serial) {
        var itemqty = document.getElementById("qty" + name + "ser" + serial);
        // 1-1.產品數量小於1，刪除該產品
        if (parseInt(itemqty.innerText) === 1) {
            // 1-1-1.拿到待刪除的節點
            var deleteItem = document.querySelector(".item" + name + "ser" + serial);
            // 1-1-2. 拿到父節點
            var parent = deleteItem.parentElement;
            // 1-1-3.删除
            parent.removeChild(deleteItem);
            if (document.getElementsByClassName("cart-item").length === 0) {
                document.querySelector(".cart-null").style.display = "";
                document.getElementById("btnSubmit").disabled = true;
            }
            setTotal();
        } // 1-2.數量遞減
        else {
            itemqty.innerText = parseInt(itemqty.innerText) - 1;
        }

        // 2.計算產品小計
        var subtotal = document.getElementById("subtotal" + name + "ser" + serial);
        subtotal.innerHTML = parseInt(subtotal.dataset.itemPrice) * parseInt(itemqty.innerText);

        setTotal();
    }

    // 購物車按+號
    function plus(name, serial) {
        // 1.數量遞增
        var itemqty = document.getElementById("qty" + name + "ser" + serial);
        itemqty.innerText = parseInt(itemqty.innerText) + 1;

        // 2.計算產品小計
        var subtotal = document.getElementById("subtotal" + name + "ser" + serial);
        subtotal.innerHTML = parseInt(subtotal.dataset.itemPrice) * parseInt(itemqty.innerText);

        setTotal();
    }

    // 計算總計
    function setTotal() {
        // 取得各產品小計
        var itemPrice = document.getElementsByClassName("item-summary-price");
        var total = 0;
        for (var intA = 0; intA < itemPrice.length; intA++) {
            total = total + parseInt(itemPrice[intA].innerText);
        }
        document.querySelector(".cart-totalPrice").innerText = total;
    }
</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<!-- Cart Remark Modal -->
<div class="modal fade" id="CartRemarkModal" tabindex="-1" role="dialog" aria-labelledby="CartRemarkModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="CartRemarkModalLabel"></h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true" style="color: #fff">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <form>
          <div class="form-group">
            <label for="message-text" class="col-form-label">請輸入備註 : </label>
            <textarea class="form-control" id="txtRemark"></textarea>
          </div>
        </form>
      </div>
      <div class="modal-footer">
        <button type="button" id="modalOk" class="btn btnsubmit btn-size-sm">儲存</button>
      </div>
    </div>
  </div>
</div>
<!-- Order Remark Modal -->
<div class="modal fade" id="OrderRemarkModal" tabindex="-1" role="dialog" aria-labelledby="OrderRemarkModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="OrderRemarkModalLabel">訂單備註</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true" style="color: #fff" >&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <p><asp:Label ID="lblAddOrderRemark" runat="server" Text=""></asp:Label></p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btnsubmit btn-size-sm" data-dismiss="modal">確定</button>
      </div>
    </div>
  </div>
</div>
  <%-- Cart --%>
   <aside>
        <form id="form1" runat="server">
            <div class="cart-container">
                <div class="cart-title text-center">
                    <h4>您的訂單<i class="icon-info-circled-alt" data-toggle="tooltip" data-placement="bottom" title="點擊品名新增備註"></i></h4>
                </div>
                <div class="cart-null text-center">無餐點項目</div> 
                <div class="cart-order" id="cartOrder" runat="server"></div>
                <div class="cart-footer">
                    <hr/>
                    <span class="clear-cart" id="clear-cart">移除全部</span>
                    <div class="cart-order-total"><b>總計 : $<span class="cart-totalPrice">0</span></b></div>
                    <div>
                        <button type="button" class="btnsubmit btnsubmit-size-lg" id="btnSubmit" disabled="disabled">提交</button>
                    </div>
                </div>
            </div>
        </form>
    </aside> 
    <%-- Order Info --%>
    <div class="order-info text-center">
        <div>
            <asp:Label ID="lblDate" class="order-time" runat="server" Text=""></asp:Label>
            <asp:Label ID="lblAddIcon" runat="server" Text=""></asp:Label>
        </div>
    </div>
  <section class="box">  
    <main id="main">
        <div class ="store-info text-center">
            <h3><asp:Label ID="lblStore" runat="server" Text=""></asp:Label></h3>
        </div>
        <nav id="categorylist" class="menu-category-list" >
            <asp:Label ID="lblAddCategorylist" runat="server" Text=""></asp:Label>
        </nav>
        <%-- Menu --%>    
        <section class="content">
            <div class="menu-items-wrapper container">
                <asp:Label ID="lblAddMenu" runat="server" Text=""></asp:Label>
            </div>
        </section>
    </main>
  </section>

<script type="text/javascript">
    // 清除購物車所有的項目
    document.getElementById("clear-cart").addEventListener('click', function () {
        var myNode = document.querySelector(".cart-order");
        while (myNode.firstChild) {
            myNode.removeChild(myNode.firstChild);
        }
        // 總計=0
        document.querySelector(".cart-totalPrice").innerText = 0;
        document.querySelector(".cart-null").style.display = "";
        document.getElementById("btnSubmit").disabled = true;
    });

    // 提交訂單
    document.getElementById("btnSubmit").addEventListener('click', function () {
        // 1.取得購物車有多少件產品
        var cartItem = document.getElementsByClassName("cart-item");

        // 2.產品資料以陣列物件方式儲存
        var Data = [];
        for (var intA = 0; intA < cartItem.length; intA++) {
            Data.push({
                "Name": document.getElementsByClassName("item-name")[intA].innerText,
                "Quantity": document.getElementsByClassName("cart-qty")[intA].innerText,
                "Price": document.getElementsByClassName("item-summary-price")[intA].dataset.itemPrice,
                "Remark": document.getElementsByClassName("item-remark")[intA].innerText
            });
        }

        console.log(Data);

        // 3.分割QueryString(uOrder.aspx?order_no=XX)，取得訂單編號
        var getParameters = location.search.split("=");

        // 4.傳遞資料至HandlerJsonString.ashx 處理
        $.ajax({
            type: "GET",
            url: "HandlerJsonString.ashx?order_no=" + getParameters[1],
            data: { order: JSON.stringify(Data) },       // JSON.stringify() 轉換成json字串
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            async: true,                
            success: function (data) {
                if (data != null) {
                    alert("提交成功！");
                    window.location.href = "uOrdered.aspx?order_no=" + getParameters[1];
                }
            },
            Error:  function (thrownError) {
                alert("Ajax Error : " + thrownError);
            }
        });
    });
</script>

</asp:Content>
