<!doctype html>
<html lang="en">
    <head>
    <title>title</title>
    <meta charset="utf-8">
    <meta name="description" content="&lt;model-viewer&gt; template">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    
	<script src="jquery-1.11.1.js"></script>
	
	<style>
	
	.login_value, .password_value {
		
	display: inline-block;

    width: 150px;	
	}
	
	.icon_close {
		
	color: red;

    font-weight: bold;
    font-family: sans-serif;

    cursor: pointer;	
	}
	
	</style>
	
    </head>
  
    <body>
    
	<h2>Панель администратора</h2>
	
	<?php 
	
	/* текущие пары логин - пароль */
	
	$fd = fopen("php/passwords.txt", 'r');
	
	while (!feof($fd)) {
		
    $str = htmlentities(fgets($fd));

    $ind = strpos($str, "---");
	
	if ($ind !== false) {
	
	$login_value = substr($str, 0, $ind);
	$password_value = substr($str, $ind+3);

	?>
	
	<p><span class="login_value"><?php echo $login_value; ?></span> - <span class="password_value"><?php echo $password_value; ?></span><span class="icon_close">X</span></p>
    
    <?php }
	
	}
	
    fclose($fd);
	
	?>
	
	<div>
	<p>Добавить нового пользователя.</p>
	<p><span class="login_value">Логин</span><input type="text" id="login_field"></p>
	<p><span class="password_value">Пароль</span><input type="text" id="password_field"></p>
	<p><button id="add_button">Добавить</button></p>
	</div>
	
	<script>
	
	if ( !sessionStorage.getItem("good_admin_auth") ) {
		
	document.location.href = "admin_panel_auth.html";	
	}
	
	$(".icon_close").each( function (ind, element) {
		
	element.onclick = function () {
		
	var login_value = element.parentElement.children[0].textContent;

	$.post('php/server.php', {delete_data: 1, login: login_value}, function (data) {
	
	if ( data.indexOf("data_deleted") != (-1) ) {
		
	alert("Данные удалены");

    setTimeout( function () {
		
	location.reload();	
		
	}, 1000);	
	
	}
	
	});
		
	};	
		
	});
	
	var add_button = $("#add_button")[0];
	
	add_button.onclick = function () {
	
	var login_value = $("#login_field")[0].value;
	var password_value = $("#password_field")[0].value;
	
	if ( (login_value) && (password_value) ) {
	
	$.post('php/server.php', {add_data: 1, login: login_value, password: password_value}, function (data) {
  
    console.log(data);
	
	if ( data.indexOf("data_added") != (-1) ) {
		
	alert("Новый пользователь добавлен");

    setTimeout( function () {
		
	location.reload();	
		
	}, 1000);	
	
	}
	
	if ( data.indexOf("data_exist") != (-1) ) {
		
	alert("Такой пользователь уже существует. Введите другие данные.");	
	}
	
	});
	
	} else {
		
	alert("Оба поля - логин и пароль обязательны для заполнения.");	
	}
	
	}
	
	</script>
	
  </body>
</html>