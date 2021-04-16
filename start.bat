echo 'Starting db...'
docker start scorp_stack_db

echo 'Starting backend...'
docker start scorp_stack_backend

echo 'Starting admin dashboard...'
docker start scorp_stack_admin_dashboard