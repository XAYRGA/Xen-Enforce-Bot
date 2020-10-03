<?php 
	define('USE_DATABASE',1);
	include 'databank.php';
	
		$sitekey = "ADD SITEKEY HERE";
	
		if (isset($_GET['success'])) {
			$success = $_GET['success'];
		} else {
			$success = 4; 
		}
		
		if (isset($_GET['reason'])) {
			$reason = $_GET['reason'];
		} else {
			$reason= "Unknown error."; 
		}
		if (isset($_GET['actid'])) {
			$actid =  mysqli_real_escape_string($DB_OBJ, $_GET['actid']);
		} else {
			$actid = "!!NONE3!!";
		}
			if ($success!=1) {
				$rpquery = "SELECT challenge,verified FROM `verify` WHERE challenge='$actid'";
				
				$res = $DB_OBJ->query($rpquery);
				if ($res==false) {
					echo($DB_OBJ->error);
die();
				}
				
				$data = mysqli_fetch_assoc($res);
				
				if (!isset($data['challenge']	)  && $actid!="!!NONE3!!") {
		
						$success=5;
						$reason="The provided activation key is not valid.";					
				
					
				} else {
					if ( $data['verified']  > 0 ) {
						$success = 6;
						$reason="This key has already been activated.";	
					}
				}
			}

					
			
	
?>
<html> 
	<head>

		<link rel="stylesheet" type="text/css" href="xga.breathe.css">
		<link rel="stylesheet" type="text/css" href="xga.nav.css">
			<!-- Bootstrap core CSS -->
	

			<!-- Custom fonts for this template -->
		<script src="http://code.jquery.com/jquery-1.7.2.js"></script>
		<link href="https://fonts.googleapis.com/css?family=Catamaran:100,200,300,400,500,600,700,800,900" rel="stylesheet">
		<link href="https://fonts.googleapis.com/css?family=Lato:100,100i,300,300i,400,400i,700,700i,900,900i" rel="stylesheet">
		<script src='https://www.hCaptcha.com/1/api.js' async defer></script>
		<link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
		<link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Raleway">
		<meta name="viewport" content="height=device-height initial-scale=1.0">

		<!-- Custom styles for this template -->
	

	  
	
	</head>
	
		
	
	
	
	<body onload="">		

	
	

		<center> 
			  <div class="w3-display-middle w3-text-white">
			  <form action="verify.php" method="post">
					<img src="/xenflogo.png" class="w3-animate-top"  height="150" width="150">
					<h1 class="w3-animate-top">Xenforce</h1>
					<p class="w3-center">Xenforce protects your telegram groups against bots and other kinds of trouble.</p>
					<hr class="w3-border-grey" style="margin:auto;width:40%">
					
					<?php 
					
						if ($actid == "!!NONE3!!") {
						
								echo('<p class="w3-center w3-red">No activation ID specified.</p>');
								die();
						}
						if ($success==1) {
							
							echo('<p class="w3-center w3-green">' . $reason . '</br>You may now close this window.</br></p>');
							die();
						}
						
						if ($success==5) {
							
							echo('<p class="w3-center w3-red">' . $reason . '</br></p>');
							die();
						}
						
						if ($success==6) {
							
							echo('<p class="w3-center w3-green">' . $reason . '</br>You may now close this window.</br></p>');
							die();
						}
						
					?>
					
					
					<p class="w3-center">The group you've attempted to join is protected by Xenforce.</p>
					<p class="w3-center">You must complete the verification below, or you'll be removed from the group.</p>
					
					
					<p class="w3-large w3-center">Please verify that you're human.</p>				
					<div class="h-captcha" data-sitekey="<?php echo $sitekey?>"</div>
					  <input type="submit" class="w3-button w3-large w3-white" value="Verify me!">
					<?php 
						echo(' <input type="hidden" id="actid" name="actid" value="' . $actid . '"> ');
						if ($success==0) {
						
							echo('<p class="w3-center w3-red">An error occured during validation: <br>' . $reason . '</br>Try again.</br></p>');
						}
						
					?>
					<br>
					<a href="index_recaptcha.php?actid=<?php echo $actid?>">Use Google Recaptcha instead</a>
				</form>
			  </div>
		</center>
		

	
		
	</body> 

	
</html>