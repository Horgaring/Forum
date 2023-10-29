if [ "$#" -lt 1 ]; then
    echo "Недостаточно аргументов"
    exit 1
fi
# identity
docker build -t forum/identity:$1  -f /home/dima/RiderProjects/Forum/Services/Identity/Dockerfile /home/dima/RiderProjects/Forum/
# apigetaway
docker build -t forum/apigeteway:$1  -f /home/dima/RiderProjects/Forum/ApiGateways/Api/Dockerfile /home/dima/RiderProjects/Forum/
# Post
docker build -t forum/post:$1  -f /home/dima/RiderProjects/Forum/Services/Forum/Api/Dockerfile /home/dima/RiderProjects/Forum/
# Comment
docker build -t forum/comment:$1  -f /home/dima/RiderProjects/Forum/Services/Comment/Api/Dockerfile /home/dima/RiderProjects/Forum/
# Notification
docker build -t forum/notification:$1  -f /home/dima/RiderProjects/Forum/Services/Notification/Api/Dockerfile /home/dima/RiderProjects/Forum/
