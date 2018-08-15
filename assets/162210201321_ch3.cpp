#include <iostream>
using namespace std;

struct Node
{
	int data;
	Node * next; 
	
}; 


class CircleList
{
	public :
		CircleList(int a[],int n);
		void EnQueqe(int x);
		int DeQueue(); 
		void show();
		
	private:
		Node * p;
		Node * rear;  
	 
};   //类声明完要加分号  

CircleList::CircleList(int a[],int n)  //带有参数的构造函数 ,尾插法建立循环链表 
{	

    p = new Node;
	rear=p;  //尾指针初始化 
 	for(int i=0;i<n;i++){
 		
 	    Node * s=new Node;	
 		s->data = a[i];
 		rear->next = s;
 	    rear=s; //将节点s插入到节点rear的后面 		
}	
   
       rear->next=p;
			 //将尾节点指向头节点 
		 		 	
}

void CircleList::EnQueqe(int x)   //入队操作，从尾节点插入数据 
{
	Node * s;            //所有出现头节点的地方用rear->next代替 
	s=new Node; 
	s->data = x;	
    s->next=rear->next;    //在尾节点处插入新节点 ，插入到尾节点与头节点之间，循环链表 
	rear->next=s;	
    rear=s;
		
}

int CircleList:: DeQueue()    //出队操作，从头节点删除数据 
{   	
		Node * q;
		q=rear->next;
		int x=q->data;
		rear->next = q->next; //摘链 
		delete q;
		return x;
						  		   	     
}

void CircleList:: show()
{
	Node * s;
    s = rear->next->next;
    while(s->next!=rear->next->next)
    {
    	cout<< s->data <<" ";
	    s = s->next;
	}
    
}


int main(){
	int a[5]={1,9,3,4,8};
	CircleList list(a,5);
     
    cout<<"原始的队列为：" <<endl;
    list.show();  
    cout<<endl;
    
    cout<<"完成出队操作后的队列为："<<endl; 
	list.DeQueue();
    list.show(); 
    cout<<endl;
    
    cout<<"完成再次出队操作后的队列为："<<endl; 
	list.DeQueue();
    list.show(); 
    cout<<endl;
    
    cout<<"完成入队操作后的队列为："<<endl; 
    list.EnQueqe(20);
    list.show();
    cout<<endl;
    
}
