let users = [1,2,3,4,5,6]
let gameRoomNumber = 1
let gameRoomObj ={}
for(let index = 0; index<users.length; index++){
  if(index%2==0){
    gameRoomObj[gameRoomNumber] = [users[index]]
    console.log(gameRoomObj)
  }
  else{
    gameRoomObj[gameRoomNumber].push(users[index])
    gameRoomNumber++
    gameRoomUser=[]
  }
}
console.log(gameRoomObj)
