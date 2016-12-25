
$(document).ready(function () {
    var suc = false;
    $('#filterPlayers').change(FilterPlayers);

    function FilterPlayers() {
        var data = $('#filterPlayers').val();
        var options = '';
        $.ajax({
            type: 'GET',
            data: { key: data },
            url: '/Players',
            success: function (result) {
                $('#heroPlayer').empty();
                options = '';
                for( i=0; i<result.name.length; i++ ){
                    options += '<option value= "' + result.player_id[i] + '">' + result.name[i] + '</option>';
                }
                console.log(options);
                $('#heroPlayer').append(options);
            }
        })
    }
    //get teams
    $.ajax({
        type: 'GET',

        url: '/Teams',
        success: function (result) {
            options = '';
            for (i = 0; i < result.name.length; i++) {
                options += '<option value= "' + result.team_id[i] + '">' + result.name[i] + '</option>';
            }

            $('#filterPlayers').append('<optgroup label="By Team">' + options + '</optgroup>');
            $('#heroTeam').append(options);
        }
    }); 
    UpdateGrid();
    var player = '';
    var player;
    var striker;
    var midfielder;
    var defender;
    var goalKeeper;
    var myPlayers;
    var mySub;
    //start showing my squad ;;
    function UpdateGrid(){
    //getting my players::
    $.ajax({
        type: 'GET',
        url: '/myPlayers',
        success: function (result) {
            
            suc = true;
             player = '';
             player = new Object();
             striker = new Array();
             midfielder = new Array();
             defender = new Array();
             goalKeeper = new Array();
             myPlayers = new Array();
             mySub = new Array();
            player+='<div class="row">'
            for (i = 0; i < result.name.length; i++) {
                myPlayers.push({
                    position: result.position[i],
                    name: result.name[i],
                    id: result.player_id[i],
                    cost: result.cost[i],
                    status: result.status[i],
                    image: "/images/player.png",
                    isPlaying:result.isPlaying[i]

                });
                if (result.isPlaying[i] == '0')
                {
                    mySub.push({
                        position: result.position[i],
                        name: result.name[i],
                        id: result.player_id[i],
                        
                        cost: result.cost[i],
                        status: result.status[i],
                        image: "/images/player.png",
                        isPlaying: result.isPlaying[i]

                    });
                    
                }
                else {

                
                if(result.position[i]=="midfielder")
                {
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
           else {goalKeeper=
            {
                position: result.position[i],
                name: result.name[i],
                id: result.player_id[i],
                cost: result.cost[i],
                status: result.status[i],
                image: "/images/player.png",
                isPlaying: result.isPlaying[i]

            }
           }
                }
            }
            WaitClick();
            // goalkeeper --  
            player= '<div class="row" style="margin-left:-9%;margin-bottom:3%;height:auto; border:1px solid yellow"><center>';
            for (i = 0; i <1; i++)
            {
             var   id =  1;
                   
             player += '<div id="' + id + '" class="player"  data-id="' + goalKeeper.id + '" data-pos="' + goalKeeper.position + '"  ">' +
'<div class="shirt  " style="border:1px solid red; ">'
             + '<img src="/images/boxing.svg" class="block"  alt="Monkey face" style="width: 30%; margin-bottom:-30%;position:relative; float:left;height: auto;">'
              
+ '<img src="/images/boxing.svg" alt="Monkey face"  style="width: 30%;  position:relative;margin-bottom:-30%;float:right;height: auto;">'
+ '<img  src="' + goalKeeper.image + '" />' 
+ '<div class="text-center playerName">' +
   '<h3 class="playerText danger">'+goalKeeper.name+'</h3> </div>'
+ '<div class="text-center playerPosition">' +
 '<h3 class="playerText danger">'+goalKeeper.position+'</h3>' +
'</div> </div> </div></center></div>';
            }
              $("#pitch").empty();
              $("#pitch").append(player);

            //end of goal keeper
            //defenders
            var defNum = parseInt(defender.length);
            var width = parseInt($(".player").css('width'));
            var margin = (100 - defNum * width) / defNum;
          
      
           // $(".player").css('margin-right', string(margin));
           
            player = '<div class="row" style="margin-left:-9%;height:auto;margin-bottom:3%; border:1px solid yellow"><center>';
            
            for (i = 0; i < defender.length; i++) {
                id ++;
                player += '<div id="' + id + '" data-id="' + defender[i].id + '" data-pos="' + defender[i].position + '" class="player defender marginButton "  >' +
              
'<div class="shirt  " style="border:1px solid red; ">'
             + '<img src="/images/boxing.svg" alt="Monkey face" class="block" style="width: 30%;  margin-bottom:-30%;position:relative; float:left;height: auto;">'
               
+ '<img src="/images/boxing.svg" alt="Monkey face" style="width: 30%;  position:relative;margin-bottom:-30%;float:right;height: auto;">'
+ '<img  src="' + defender[i].image + '" class=" "/>'
+ '<div class="text-center playerName">' +
   '<h3 class="playerText danger">' + defender[i].name + '</h3> </div>'
+ '<div class="text-center playerPosition">' +
 '<h3 class="playerText danger">' + defender[i].position + '</h3>' +
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
                player += '<div id="' + id + '" data-id="' + midfielder[i].id + '" data-pos="' + midfielder[i].position + '"  class="player midfielder marginButton  " >' +

'<div class="shirt  " style="border:1px solid red; ">'
             + '<img src="/images/boxing.svg" alt="Monkey face" class="block"  style="width: 30%;   margin-bottom:-30%;position:relative; float:left;height: auto;">'

+ '<img src="/images/boxing.svg" alt="Monkey face" style="width: 30%;position:relative;margin-bottom:-30%;float:right;height: auto;">'
+ '<img  src="' + midfielder[i].image + '" class=" "/>'
+ '<div class="text-center playerName">' +
   '<h3 class="playerText danger">' + midfielder[i].name + '</h3> </div>'
+ '<div class="text-center playerPosition">' +
 '<h3 class="playerText danger">' + midfielder[i].position + '</h3>' +
'</div> </div> </div>';

            }
            player += '</center></div>';

            $("#pitch").append(player);
            $(".midfielder").css('margin', (midfieldNum-screen.width/1000+'%'));
            //end of midfield


            player = '<div class="row" style="margin-left:-9%;height:auto;  margin-bottom:3%; border:1px solid yellow"><center>';

            for (i = 0; i < striker.length; i++) {
                id++;
                player += '<div id="' + id + '"data-id="' + striker[i].id + '" data-pos="' + striker[i].position + '"  class="player striker marginButton  " >' +

'<div class="shirt  " style="border:1px solid red; ">'
             + '<img src="/images/boxing.svg" alt="Monkey face" class="block" style="width: 30%;  margin-bottom:-30%;position:relative; float:left;height: auto;">'

+ '<img src="/images/boxing.svg" alt="Monkey face" style="width: 30%;  position:relative;margin-bottom:-30%;float:right;height: auto;">'
+ '<img  src="' + striker[i].image + '"class=" " />'
+ '<div class="text-center playerName">' +
   '<h3 class="playerText danger">' + striker[i].name + '</h3> </div>'
+ '<div class="text-center playerPosition">' +
 '<h3 class="playerText danger">' + striker[i].position + '</h3>' +
'</div> </div> </div>';

            }
            player += '</center></div>';

            $("#pitch").append(player);
            var striker = parseInt(striker.length);
            $(".striker").css('margin', (striker - screen.width / 1000 + '%'));

            //loading sub::
            player = '';
            player += '<div class="row "  >';

            for(i=0; i<mySub.length; i++)
            {
                id++;
                player += '<div id="' + id + '" data-pos="' + mySub[i].position + '" class="col-xs-6 "data-id="' + mySub[i].id + '" style="  background-colborder:1px solid black;">' +
                    '<div class="col-xs-6 text-center  mg" >' +
                    '<img src="/images/boxing.svg" class="img-responsive block "  ></div>' +
                '<div class="col-xs-6 text-center mg " >' +
                '<img src="/images/info.svg" class="img-responsive "  ></div>' +
                '<img src="' + mySub[i].image + ' " class="img-responsive" />' +
                
                '<div class="col-xs-12 text-center  " style="background-color:lightblue;">'+
                    mySub[i].name+   '</div>' +
                 '<div class="col-xs-12 text-center  " style="background-color:cadetblue;">' +
                 mySub[i].position+'</div>' +
                '</div>';
               
               
          
        }
            player += '</div>';

            $("#sub").empty();
            $("#sub").append(player);
        }
    });
    }


    $('body').delegate('.block', 'click', function () {
       
        HandleClick($(this).parent().parent());


    });
    function WaitClick()
    {
        defN = parseInt(defender.length);
        midfN = parseInt(midfielder.length);
        goalN = 1;
        strN = 10 - defN - midfN;
        firstPlayer = 0;
        secondPlayer = 0;
     
      
    }
   
     
  
        function HandleClick(target)
        {
            var id = parseInt(target.attr("id"));
            

            if (id>=12)
            {
                for(i=12; i<=15; i++)
                {
                    if(i!=id)
                        $('#' + i + '').removeClass('clickEffect');
                    else {
                        if (target.hasClass('clickEffect'))
                        {
                            firstPlayer = 0;

                        } else {
                            firstPlayer = parseInt(target.data("id"));
                            firstPlayerId = id;
                          
                        }
                        target.toggleClass('clickEffect');
                    }
                }
               

                

            }  //if second player (player in formation ..)
            else {
                for (i = 1; i <= 11; i++) {
                    if (i != id)
                        $('#' + i + '').removeClass('clickEffectS');
                    else {
                        if (target.hasClass('clickEffectS')) {
                            secondPlayer = 0;
                            
                        } else {
                            secondPlayer = parseInt(target.data("id"));
                            secondPlayerId = id;
                           
                        }
                        target.toggleClass('clickEffectS');
                    }
                }


            }
          //  firstPlayer = target.parent().parent().data('id');
         //   alert(target.parent().parent().attr("id"));
            // $(this).parent().parent().toggleClass('clickEffect');
            
            if (firstPlayer != 0 && secondPlayer != 0)
            {//get type 1 and type 2 in grid system...;

                typeP1N = 0;
                typeP2N = 0;
                
                if ($('#' + firstPlayerId + '').data("pos") == $('#' + secondPlayerId + '').data("pos"))
                {
                    Swap(firstPlayer, secondPlayer);
                }


                if($('#' + firstPlayerId + '').data("pos")=="defender")
                {
                    typeP1N = defN + 1;

                }
               
                if ($('#' + firstPlayerId + '').data("pos") == "midfielder") {
                    typeP1N = midfN + 1;

                }
                
                if ($('#' + firstPlayerId + '').data("pos") == "striker") {
                    typeP1N = strN + 1;

                }
                
               
                //type2(player2)(in the grid System);              

                if ($('#' + secondPlayerId + '').data("pos") == "defender") {
                    typeP2N = defN - 1;

                }
                
                if ($('#' + secondPlayerId + '').data("pos") == "midfielder") {
                    typeP2N = midfN - 1;

                }
                if ($('#' + secondPlayerId + '').data("pos") == "striker") {
                    typeP2N = strN - 1;

                }
              
                if(typeP1N!=0&&typeP2N!=0&&typeP1N>=2&&typeP1N<=4&&typeP2N>=2&&typeP1N<=4)
                {
                   
                    Swap(firstPlayer, secondPlayer);
                }
               
                
             
               
            }
        }


        function Swap(p1,p2)
        {   
           
            $.ajax({
                contentType: 'application/json',
                type: 'POST',
                data: JSON.stringify({
                    primary: p1,
                    secondary: p2
                }),
               
                url: 'Game/UpdateTempFormation',
                success: function () {
                    for (j = 1; j <= 11; j++) {
                        $('#' + j + '').removeClass('clickEffectS');
                    }
                    for (v = 12; v <= 15; v++) {
                        $('#' + v + '').removeClass('clickEffect');
                    }
                    firstPlayer = 0;
                    secondPlayer = 0;
                    UpdateGrid();
                }
            });



        }


















});