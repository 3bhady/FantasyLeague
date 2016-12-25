
$(document).ready(function () {
    var oldPlayers = new Array();
    var newPlayers = new Array();

    var players = new Array();
    var tempPlayers = new Array();
    var defN = 0;
    var strN = 0;
    var midfN = 0;
    var gkN = 0;
    var firstPlayer = 0;
    var secondPlayer = 0;

    $.ajax({
        type: 'GET',

        url: '/Teams',
        success: function (result) {
            options = '';
            for (i = 0; i < result.name.length; i++) {
                options += '<option value= "' + result.team_id[i] + '">' + result.name[i] + '</option>';
            }

            $('#filterPlayers').append('<optgroup label="By Team">' + options + '</optgroup>');
            
        }
    });

    $('#filterPlayers').change(FilterPlayers);
    function FilterPlayers() {
         data = $('#filterPlayers').val();
        var options = '';
        $.ajax({
            type: 'GET',
            data: { key: data },
            url: '/Players',
            success: function (result) {
               // console.log(result); 
              UpdatePlayersList(result);
            }
        })
    }

    UpdateGrid();
    function UpdateMoney(value,op)
    {
     
        var toAdd = parseInt(value);
        var currentValue = parseInt($("#money").text());
        if (op == "n")
        {
            $("#money").text(currentValue - toAdd);
        }
        else {
            $("#money").text(currentValue + toAdd );
        }
       
     
    }
    $('body').delegate('.block', 'click', function () {
        //alert($(this).parent().parent().data("id"));
        //  HandleClick($(this).parent().parent());
        var id = $(this).parent().parent().attr("id");//card id(contain all info)
        var target = $($(this).parent().children()[2]);
        var parent = target.parent().parent();
        var moneyToAdd = parseInt(parent.data("cost"));
        if (parent.data("rec")=="1") {
            secondPlayer = 0;
            parent.data("rec", "0");
            target.attr("src", parent.data("image"));
            UpdateMoney(moneyToAdd, "n");
           // alert("sub");
        } else {
            parent.data("rec", "1");
            
            for (i = 1; i <= 15; i++)
            {if(i!=id)
                $('#' + i + '').removeClass("clickEffect");
            }
            target.attr("src", "/images/shirt.png");
            secondPlayer = id;
           
            UpdateMoney(moneyToAdd, "p");
          //  alert("add");

           
        }
        $('#' + id + '').toggleClass("clickEffect");
      

    });
    //handle exchange ...
    $('body').delegate('.toBuy', 'click', function () {
        var subCost = parseInt($(this).data("cost"));
        var subImage = $(this).data("image");
        var subPosition = $(this).data("pos");
        var subId = $(this).data("id");
        var subName = $(this).data("name");
        var dup = false;
        var rep = 0;
        for (i = 1; i <= 15; i++) {
            if ($('#' + i + '').data("id") == subId) {
                dup = true;
            }
            if($('#'+i+'').data("rec")=="1")
            {
                if ($('#' + i + '').data("pos") == subPosition)
                {
                    rep = i;
                }
               
                
            }
        }
      
        if(!dup && rep!=0)
        {
          
            rep = parseInt(rep);
            $('#' + String(rep ) + '').data("rec", "0");
            $('#' + String(rep ) + '').data("cost", $(this).data("cost"));
            $('#' + String(rep ) + '').data("name", subName);
            $('#' + String(rep ) + '').data("image", subImage);
            $('#' + String(rep ) + '').data("id", subId);
            $('#' + String(rep + 20) + '').empty().append(subName);
            $('#' + String(rep + 40) + '').text(subCost + 'k');
            $($('#' + String(rep) + '').children().children()[2]).attr("src", subImage);
            $('#' + String(rep) + '').removeClass("clickEffect");
            UpdateMoney(subCost,"n");
            firstPlayer = 0;
            secondPlayer = 0;
          //  alert($($('#' + String(rep) + '').children().children()[2]).attr("src"));
           // alert(subName);
         //   alert(rep+20);
          //  alert($('#' + rep + 40 + '').val(subCost));
            dup = false;
            rep = 0;
           // alert("i got a mate ...");
        }

    });
   





















































































































































    function UpdatePlayersList(result) {
        
        var option = '<div   class="row pre-scrollable" ><table   class = "table  table-striped table-hover "><tbody  id="inventor">' +
  '<caption>' + data + '</caption> <thead> <tr> <th> Player Name</th> <th>Position</th><th>Cost</th><th>Team</tr></thead>';
        for( i=0; i<result.pname.length; i++ )
        {
            option += '<tr class ="toBuy"  style="cursor:pointer"  data-id= "' + result.player_id[i] + '" data-name= "' + result.pname[i] + '"' +
             'data-cost= "' + result.cost[i] + '" data-pos= "' + result.position[i] + '" ' +
             ' data-image="'+result.pimage[i]+'">' +
            '<td ><div  >' + result.pname[i] + '</div></td>' +
            '<td>'+result.position[i]+'</td>' +
            '<td>' + result.cost[i]  + 'k</td>' +
            '<td>' + result.tname[i] + '</td>' 

         '</tr>';
           
        }
      
       
        option += '</tbody></table></div>';
        $("#players").empty().append(option);
       
    }




    (function UpdateUserInfo()
    {
        $.ajax({
            type: 'GET',
         
            url: '/UpdateUserInfo',
            success: function (result) {
                $("#username").text(String( result.username));
                $("#points").text(result.points);
                $("#money").text(result.money);
               
            }
        })

    })();
















       
        //get teams
       
        var player = '';
        var player;
        var striker;
        var midfielder;
        var defender;
        var goalKeeper;
        var myPlayers;
        var mySub;
        //start showing my squad ;;
    
        function UpdateGrid() {
            //getting my players::
            $.ajax({
                type: 'GET',
                url: '/myPlayers',
                success: function (result) {
                  
                   // console.log(result);
                    suc = true;
                    player = '';
                    player = new Object();
                    striker = new Array();
                    midfielder = new Array();
                    defender = new Array();
                    goalKeeper = new Array();
                    myPlayers = new Array();
                    mySub = new Array();
                    player += '<div class="row">'
                    for (i = 0; i < result.name.length; i++) {
                        myPlayers.push({
                            position: result.position[i],
                            name: result.name[i],
                            id: result.player_id[i],
                            cost: result.cost[i],
                            status: result.status[i],
                            image: "/images/player.png",
                            isPlaying: result.isPlaying[i]

                        });
                        oldPlayers.push(result.player_id);
                        if (result.position[i] == "midfielder") {
                            midfielder.push(
                                {
                                    position: result.position[i],
                                    name: result.name[i],
                                    id: result.player_id[i],
                                    cost: result.cost[i],
                                    status: result.status[i],
                                    image: "/images/player.png",
                                    isPlaying: result.isPlaying[i]

                                }
                                );

                        }
                        else if (result.position[i] == "striker") {
                            striker.push(
                                   {
                                       position: result.position[i],
                                       name: result.name[i],
                                       id: result.player_id[i],
                                       cost: result.cost[i],
                                       status: result.status[i],
                                       image: "/images/player.png",
                                       isPlaying: result.isPlaying[i]

                                   }
                                   );

                        }
                        else if (result.position[i] == "defender") {
                            defender.push(
                                       {
                                           position: result.position[i],
                                           name: result.name[i],
                                           id: result.player_id[i],
                                           cost: result.cost[i],
                                           status: result.status[i],
                                           image: "/images/player.png",
                                           isPlaying: result.isPlaying[i]

                                       }
                                       );

                        }
                        else {
                            goalKeeper.push(
                             {
                                 position: result.position[i],
                                 name: result.name[i],
                                 id: result.player_id[i],
                                 cost: result.cost[i],
                                 status: result.status[i],
                                 image: "/images/player.png",
                                 isPlaying: result.isPlaying[i]

                             });
                        }
                    }
                  //  console.log(defender);

                    // goalkeeper --   
                    var id = 0;
                    var defNum = parseInt(defender.length);
                    var width = parseInt($(".player").css('width'));
                    var margin = (100 - defNum * width) / defNum;


                    // $(".player").css('margin-right', string(margin));

                    player = '<div class="row" style="margin-left:-9%;height:auto;margin-bottom:3%; border:1px solid yellow"><center>';

                    for (i = 0; i < goalKeeper.length; i++) {
                        id++;
                        player += '<div data-rec ="0" ' +
                            ' id="' + id + '" data-cost="' + goalKeeper[i].cost + '" data-id="' + goalKeeper[i].id + '" data-pos="' + goalKeeper[i].position + '" ' +
                            'data-isplaying="' + goalKeeper[i].isPlaying + '" data-oldid="' + goalKeeper[i].id + '"' +
                            'data-image="'+goalKeeper[i].image+'" class="player defender marginButton "  >' +

        '<div class="shirt  " style="border:1px solid red; ">'
                     + '<img src="/images/boxing.svg" alt="Monkey face" class="block" style="width: 30%;  margin-bottom:-30%;position:relative; float:left;height: auto;">'

        + '<img src="/images/boxing.svg" alt="Monkey face" style="width: 30%;  position:relative;margin-bottom:-30%;float:right;height: auto;">'
        + '<img  src="' + goalKeeper[i].image + '" class=" "/>'
        + '<div class="text-center playerName">' +
           '<h3 id="'+(parseInt(id)+20)+'" class="playerText danger">' + goalKeeper[i].name + '</h3> </div>'
        + '<div class="text-center playerPosition">' +
         '<h3 class="playerText danger">' + goalKeeper[i].position + '</h3>' +
         '<div class="text-center playerName">' +
           '<h3 id="' + (parseInt(id) + 40) + '" class="playerText danger">' + goalKeeper[i].cost +'k'+ '</h3> </div>' +
        '</div> </div> </div>';

                    }
                    player += '</center></div>';

                    $("#pitch").append(player);
                    //   $(".defender").css('margin', (defNum * 3));
                    $(".goalKeeper").css('margin', (defNum - screen.width / 1000 + '%'));

                    //end of goal keeper
                    //defenders
                    var defNum = parseInt(defender.length);
                    var width = parseInt($(".player").css('width'));
                    var margin = (100 - defNum * width) / defNum;


                    // $(".player").css('margin-right', string(margin));

                    player = '<div class="row" style="margin-left:-9%;height:auto;margin-bottom:3%; border:1px solid yellow"><center>';

                    for (i = 0; i < defender.length; i++) {
                        id++;
                        player += '<div data-rec ="0"  data-image="' + defender[i].image + '" data-cost="' + defender[i].cost + '" ' +
                            'data-isplaying="' + defender[i].isPlaying + '" data-oldid="' + defender[i].id + '" ' +
                            ' id="' + id + '" data-id="' + defender[i].id + '" data-pos="' + defender[i].position + '"  class="player midfielder marginButton  " >' +

        '<div class="shirt  " style="border:1px solid red; ">'
                     + '<img src="/images/boxing.svg" alt="Monkey face" class="block"  style="width: 30%;   margin-bottom:-30%;position:relative; float:left;height: auto;">'

        + '<img src="/images/boxing.svg" alt="Monkey face" style="width: 30%;position:relative;margin-bottom:-30%;float:right;height: auto;">'
        + '<img  src="' + defender[i].image + '" class=" "/>'
        + '<div class="text-center playerName">' +
           '<h3 id="' + (parseInt(id) + 20) + '"  class="playerText danger">' + defender[i].name + '</h3> </div>'
        + '<div class="text-center playerPosition">' +
         '<h3 class="playerText danger">' + defender[i].position + '</h3>' +
         '<div class="text-center playerName">' +
           '<h3 id="' + (parseInt(id) + 40) + '" class="playerText danger">' + defender[i].cost + 'k' + '</h3> </div>' +
        '</div> </div> </div>';

                    }
                    player += '</center></div>';

                    $("#pitch").append(player);
                    //   $(".defender").css('margin', (defNum * 3));
                    $(".defender").css('margin', (defNum - screen.width / 1000 + '%'));
                    //end of defenders
                    //midfielders
                    var midfieldNum = parseInt(midfielder.length);



                    // $(".player").css('margin-right', string(margin));

                    player = '<div class="row" style="margin-left:-9%;height:auto;  margin-bottom:3%; border:1px solid yellow"><center>';

                    for (i = 0; i < midfielder.length; i++) {
                        id++;
                        player += '<div data-rec ="0"  data-image="' + midfielder[i].image + '" data-cost="' + midfielder[i].cost + '" ' +
                            'data-isplaying="' + midfielder[i].isPlaying + '" data-oldid="' + midfielder[i].id + '" ' +
                            ' id="' + id + '" data-id="' + midfielder[i].id + '" data-pos="' + midfielder[i].position + '"  class="player midfielder marginButton  " >' +

        '<div class="shirt  " style="border:1px solid red; ">'
                     + '<img src="/images/boxing.svg" alt="Monkey face" class="block"  style="width: 30%;   margin-bottom:-30%;position:relative; float:left;height: auto;">'

        + '<img src="/images/boxing.svg" alt="Monkey face" style="width: 30%;position:relative;margin-bottom:-30%;float:right;height: auto;">'
        + '<img  src="' + midfielder[i].image + '" class=" "/>'
        + '<div class="text-center playerName">' +
           '<h3 id="' + (parseInt(id) + 20) + '"  class="playerText danger">' + midfielder[i].name + '</h3> </div>'
        + '<div class="text-center playerPosition">' +
         '<h3 class="playerText danger">' + midfielder[i].position + '</h3>' +
         '<div class="text-center playerName">' +
           '<h3 id="' + (parseInt(id) + 40) + '" class="playerText danger">' + midfielder[i].cost + 'k' + '</h3> </div>' +
        '</div> </div> </div>';

                    }
                    player += '</center></div>';

                    $("#pitch").append(player);
                    $(".midfielder").css('margin', (midfieldNum - screen.width / 1000 + '%'));
                    //end of midfield


                    player = '<div class="row" >';

                    for (i = 0; i < striker.length; i++) {
                        id++;
                        player += '<div  data-rec ="0"  data-image="' + striker[i].image + '" data-isplaying="' + striker[i].isPlaying + '" ' +
                            ' data-cost="' + striker[i].cost + '"data-oldid="' + striker[i].id + '" ' +
                            ' id="' + id + '"data-id="' + striker[i].id + '" data-pos="' + striker[i].position + '"  class="player striker marginButton   " >' +

        '<div class="shirt  " style="border:1px solid red; ">'
                     + '<img src="/images/boxing.svg" alt="Monkey face" class="block" style="width: 30%;  margin-bottom:-30%;position:relative; float:left;height: auto;">'

        + '<img src="/images/boxing.svg" alt="Monkey face" style="width: 30%;  position:relative;margin-bottom:-30%;float:right;height: auto;">'
        + '<img  src="' + striker[i].image + '"class=" " />'
        + '<div class="text-center playerName">' +
           '<h3 id="' + (parseInt(id) + 20) + '"  class="playerText danger">' + striker[i].name + '</h3> </div>'
        + '<div class="text-center playerPosition">' +
         '<h3 class="playerText danger">' + striker[i].position + '</h3>' +
          '<h3 id="' + (parseInt(id) + 40) + '" class="playerText danger">' + striker[i].cost + 'k' + '</h3> </div>' +
        '</div>  </div>';

                    }
                    //player += '</div>';

                    $("#pitch").append(player);
                    var striker = parseInt(striker.length);
                  $(".striker").css('margin', (4+ '%'));

          

                    $("#sub").empty();
                    $("#sub").append(player);
                    $(".player").draggable(function () { });
                    $("#userInfo").draggable(function () { });
                    $("#save").click(function () {
                        Check();
                    });
                }
            });
        }


        function Check()
        {error=false;
            for (i = 1; i <= 15; i++) {
               
                if($('#' + i + '').data("rec")=="1")
                {
                    error = true;
                    alert("cannot save because of null players ");
                }
                else if(parseInt($("#money").text())<0)
                {
                    error = true;
                    alert("with negative money no one would accept that ..");
                }
            }
            if(!error)
            {
                //alert("every thing is ok lets work ..");
                for(i=1; i<=15; i++)
                {
                    newPlayers.push(
                            $('#' + i + '').data("oldid"), $('#' + i + '').data("id")
                        )
                }

                newPlayers.push($("#money").text());
                alert($("#money").text());
                //sending data ::
                $.ajax({
                    contentType: 'application/json',
                    type: 'POST',
                    data: JSON.stringify( newPlayers),
                    url: '/game/UpdateFormation',
                    success: function (result) {
                        //alert("success!!")
                        window.location.href = '/home/index';
                    }
                });


            }
           
        }










    });